using System.Collections.Generic;
using System.Linq;
using System;

namespace Putovanja
{
    public static class DatabaseContext
    {
        public static bool RegisterMember(string name, string surname, string email, string password, out string errorMessage)
        {
            errorMessage = string.Empty;
            try
            {
                using (var context = new PlaniranjePutovanjaEntities7())
                {
                    // Provera da li email već postoji u tabeli Korisnik
                    if (context.Korisnik.Any(k => k.Email == email))
                    {
                        errorMessage = "Email adresa već postoji.";
                        return false;
                    }

                    // Provera da li email već postoji u tabeli Agencija
                    if (context.Agencija.Any(a => a.Email == email))
                    {
                        errorMessage = "Email adresa već postoji.";
                        return false;
                    }

                    // Hashovanje lozinke
                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

                    var newKorisnik = new Korisnik
                    {
                        Ime = name,
                        Prezime = surname,
                        Lozinka = hashedPassword,
                        Email = email
                    };

                    context.Korisnik.Add(newKorisnik);
                    context.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = "Greška prilikom registracije: " + ex.Message;
                return false;
            }
        }


        public static bool RegisterAgency(string agencyName, string email, string password, out string errorMessage)
        {
            errorMessage = string.Empty;
            try
            {
                using (var context = new PlaniranjePutovanjaEntities7())
                {
                    // Provera da li email već postoji u tabeli Korisnik
                    if (context.Korisnik.Any(k => k.Email == email))
                    {
                        errorMessage = "Email adresa već postoji.";
                        return false;
                    }

                    // Provera da li email već postoji u tabeli Agencija
                    if (context.Agencija.Any(a => a.Email == email))
                    {
                        errorMessage = "Email adresa već postoji.";
                        return false;
                    }

                    // Hashovanje lozinke
                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

                    var newAgency = new Agencija
                    {
                        Naziv = agencyName,
                        Lozinka = hashedPassword,
                        Email = email
                    };

                    context.Agencija.Add(newAgency);
                    context.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = "Greška prilikom registracije: " + ex.Message;
                return false;
            }
        }

        // Metoda za upisivanje destinacije u bazu
        public static bool InsertDestination(Destinacija destinacija, out string errorMessage)
        {
            errorMessage = string.Empty;

            try
            {
                using (var context = new PlaniranjePutovanjaEntities7())
                {
                    // Dodajte novu destinaciju u DbSet
                    context.Destinacija.Add(destinacija);

                    // Sačuvajte promene u bazi
                    context.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {
                errorMessage = "Greška prilikom dodavanja destinacije: " + ex.Message;
                return false;
            }
        }


        public static bool AuthenticateUser(string email, string password, out string userType, out int userId)
        {
            using (var context = new PlaniranjePutovanjaEntities7())
            {
                userType = null;
                userId = 0;

                var user = context.Korisnik.FirstOrDefault(k => k.Email == email);
                if (user != null)
                {
                    if (BCrypt.Net.BCrypt.Verify(password, user.Lozinka))
                    {
                        userType = "Korisnik";
                        userId = user.idKorisnik;
                        return true;
                    }
                }

                var agency = context.Agencija.FirstOrDefault(a => a.Email == email);
                if (agency != null)
                {
                    if (BCrypt.Net.BCrypt.Verify(password, agency.Lozinka))
                    {
                        userType = "Agencija";
                        userId = agency.idAgencija;
                        return true;
                    }
                }

                return false;
            }
        }

        public static bool InsertTravel(Putovanje putovanje, out string errorMessage)
        {
            errorMessage = string.Empty;

            try
            {
                using (var context = new PlaniranjePutovanjaEntities7())
                {
                    // Dodajte novo putovanje u DbSet
                    context.Putovanje.Add(putovanje);

                    // Sačuvajte promene u bazi
                    context.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {
                errorMessage = "Greška prilikom dodavanja putovanja: " + ex.Message;
                return false;
            }
        }

        public static List<Putovanje> GetPutovanja()
        {
            using (var context = new PlaniranjePutovanjaEntities7())
            {
                return context.Putovanje.ToList();
            }
        }

        public static List<Putovanje> GetPutovanjaId(int idAgencija)
        {
            using (var context = new PlaniranjePutovanjaEntities7())
            {
                return context.Putovanje.Where(p => p.IdAgencija == idAgencija).ToList();
            }
        }

        public static void DeletePutovanje(int putovanjeId)
        {
            try
            {
                using (var context = new PlaniranjePutovanjaEntities7())
                {
                    var putovanjeToDelete = context.Putovanje.Find(putovanjeId);
                    if (putovanjeToDelete != null)
                    {
                        context.Putovanje.Remove(putovanjeToDelete);
                        context.SaveChanges();
                    }
                    else
                    {
                        throw new InvalidOperationException($"Putovanje with ID {putovanjeId} not found.");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting putovanje.", ex);
            }
        }


        public static bool InsertReservation(int idKorisnik, int idPutovanja, out string errorMessage)
        {
            errorMessage = string.Empty;

            try
            {
                using (var context = new PlaniranjePutovanjaEntities7())
                {
                    // Provera da li korisnik već ima rezervaciju za dato putovanje
                    bool hasReservation = context.Rezervacija.Any(r => r.idKorisnik == idKorisnik && r.idPutovanja == idPutovanja);
                    if (hasReservation)
                    {
                        errorMessage = "Korisnik već ima rezervaciju za ovo putovanje.";
                        return false;
                    }

                    // Kreiranje nove rezervacije
                    var newReservation = new Rezervacija
                    {
                        idKorisnik = idKorisnik,
                        idPutovanja = idPutovanja
                    };

                    // Dodavanje rezervacije u DbSet
                    context.Rezervacija.Add(newReservation);

                    // Smanjenje broja slobodnih mesta za jedan
                    var putovanje = context.Putovanje.FirstOrDefault(p => p.idPutovanja == idPutovanja);
                    if (putovanje != null && putovanje.BrojPutnika > 0)
                    {
                        putovanje.BrojPutnika--;

                        // Ažuriranje statusa putovanja ako su sva mesta popunjena
                        if (putovanje.BrojPutnika == 0)
                        {
                            putovanje.Status = "Sva mesta su popunjena";
                        }

                        // Čuvanje promena u bazi podataka
                        context.SaveChanges();
                    }
                    else
                    {
                        errorMessage = "Nema slobodnih mesta za rezervaciju.";
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                errorMessage = "Greška prilikom dodavanja rezervacije: " + ex.Message;
                return false;
            }
        }

        public static Putovanje GetPutovanjeById(int idPutovanja)
        {
            using (var context = new PlaniranjePutovanjaEntities7())
            {
                return context.Putovanje.FirstOrDefault(p => p.idPutovanja == idPutovanja);
            }
        }

        public static bool UpdatePutovanje(Putovanje putovanje, out string errorMessage)
        {
            errorMessage = string.Empty;

            try
            {
                using (var context = new PlaniranjePutovanjaEntities7())
                {
                    var existingPutovanje = context.Putovanje.FirstOrDefault(p => p.idPutovanja == putovanje.idPutovanja);
                    if (existingPutovanje != null)
                    {
                        // Ažuriranje podataka o putovanju
                        context.Entry(existingPutovanje).CurrentValues.SetValues(putovanje);
                        context.SaveChanges();
                        return true;
                    }
                    else
                    {
                        errorMessage = "Putovanje nije pronađeno.";
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = "Greška prilikom ažuriranja putovanja: " + ex.Message;
                return false;
            }
        }

        public static List<dynamic> GetPutniciByPutovanjeId(int idPutovanja, out string errorMessage)
        {
            errorMessage = string.Empty;

            try
            {
                using (var context = new PlaniranjePutovanjaEntities7())
                {
                    // Dohvati putnike koji su rezervisali odabrano putovanje
                    var putnici = context.Rezervacija
                        .Where(r => r.idPutovanja == idPutovanja)
                        .Select(r => new
                        {
                            Ime = r.Korisnik.Ime,
                            Prezime = r.Korisnik.Prezime,
                            Email = r.Korisnik.Email
                        })
                        .ToList();

                    return new List<dynamic>(putnici);
                }
            }
            catch (Exception ex)
            {
                errorMessage = $"Greška prilikom učitavanja putnika: {ex.Message}";
                return null;
            }
        }

        public static List<dynamic> GetReservationsForUser(int userId, out string errorMessage)
        {
            errorMessage = string.Empty;

            try
            {
                using (var context = new PlaniranjePutovanjaEntities7())
                {
                    // Dohvati rezervacije za ulogovanog korisnika
                    var reservations = context.Rezervacija
                        .Where(r => r.idKorisnik == userId)
                        .Select(r => new
                        {
                            Naziv = r.Putovanje.Naziv,
                            Opis = r.Putovanje.Opis,
                            Cena = r.Putovanje.Cena,
                            Slika = r.Putovanje.Slika,
                            DatumPolaska = r.Putovanje.DatumPolaska,
                            DatumPovratka = r.Putovanje.DatumPovratka,
                            IdPutovanja = r.idPutovanja // Promena u IdPutovanja da se izbegne konfuzija sa anonimnim tipom
                        })
                        .ToList();

                    // Obrnuti redosled elemenata
                    reservations.Reverse();

                    return new List<dynamic>(reservations);
                }
            }
            catch (Exception ex)
            {
                errorMessage = $"Greška prilikom učitavanja rezervacija: {ex.Message}";
                return null;
            }
        }

        public static Putovanje GetTravelDetails(int idPutovanja, out string errorMessage)
        {
            errorMessage = string.Empty;

            try
            {
                using (var context = new PlaniranjePutovanjaEntities7())
                {
                    return context.Putovanje.FirstOrDefault(p => p.idPutovanja == idPutovanja);
                }
            }
            catch (Exception ex)
            {
                errorMessage = $"Greška prilikom učitavanja detalja putovanja: {ex.Message}";
                return null;
            }
        }

        public static List<Destinacija> GetDestinationsForTravel(int idPutovanja, out string errorMessage)
        {
            errorMessage = string.Empty;

            try
            {
                using (var context = new PlaniranjePutovanjaEntities7())
                {
                    return context.Destinacija.Where(d => d.idPutovanja == idPutovanja).ToList();
                }
            }
            catch (Exception ex)
            {
                errorMessage = $"Greška prilikom učitavanja destinacija: {ex.Message}";
                return null;
            }
        }

        public static bool CheckReservation(int idKorisnik, int idPutovanja)
        {
            using (var context = new PlaniranjePutovanjaEntities7())
            {
                return context.Rezervacija.Any(r => r.idKorisnik == idKorisnik && r.idPutovanja == idPutovanja);
            }
        }
    }
}