Klamminger: Gutes Read me

# Online Bookshelf Library

Willkommen zur **Online Bookshelf Library** – einer modernen Webanwendung zur Verwaltung, Ausleihe und Reservierung von Büchern für Kunden und Mitarbeiter.

---

## Features

### Für alle Benutzer
- **Landingpage**: Übersichtliches Karussell mit Buchgenres und Suchfunktion nach Titel, Autor oder Genre.
- **Responsive Design**: Optimiert für Desktop und mobile Geräte.
- **Benutzerfreundliche Navigation**: Intuitive Menüführung und klare Seitenstruktur.

### Für Kunden
- **Registrierung & Login**: Sichere Anmeldung und Registrierung mit E-Mail, Passwort und weiteren Angaben.
- **Buchsuche**: Schnelle Suche nach Büchern mit Filtermöglichkeiten.
- **Buchliste**: Eigene Merkliste für Bücher, die ausgeliehen oder vorgemerkt werden sollen.
- **Reservierungen**: Bücher reservieren und Abholfristen einsehen.
- **Ausleihen**: Übersicht über ausgeliehene Bücher und Rückgabefristen.

### Für Mitarbeiter
- **Buchverwaltung**: Bücher hinzufügen, bearbeiten und löschen.
- **Ausleihübersicht**: Alle ausgeliehenen Bücher und deren Rückgabestatus einsehen.
- **Benutzerverwaltung**: Kunden- und Mitarbeiterdaten verwalten.

---

## Projektstruktur

- **/Pages**: Razor Pages für alle Ansichten (Landingpage, Login, Buchlisten, Verwaltung etc.)
- **/Data**: Repositories und Services für Datenzugriff und Geschäftslogik (`BookRepository`, `UserRepository`, `AuthService` usw.)
- **/Shared**: Geteilte Komponenten und Layouts (z.B. Navigation, MainLayout)
- **/wwwroot**: Statische Dateien wie CSS, Bilder und Icons
- **/Database.cs**: Initialisierung und Verwaltung der SQLite-Datenbank

---

## Wichtige Attribute & Datenbankstruktur

### User
- **UserID**: Primärschlüssel
- **Name, Email, Password, Role, Phone, Address**: Stammdaten & Rechte

### Book
- **BookID**: Primärschlüssel
- **Title, Author, Genre, Summary**: Buchinformationen
- **IsAvailable**: Verfügbarkeit (true/false)
- **ImageBase64**: (optional) Bild als Base64-String für Buchcover

### Reservation
- **ReservationID**: Primärschlüssel
- **UserID, BookID**: Fremdschlüssel
- **ReservationAt, PickupDeadline**: Zeitliche Angaben

### Loan
- **LoanID**: Primärschlüssel
- **BookID, UserID**: Fremdschlüssel
- **LoanDate, DueDate, ReturnedDate, Extended**: Ausleihstatus

---

## Installation & Start

1. **Voraussetzungen**: [.NET 7+ SDK](https://dotnet.microsoft.com/download), [SQLite](https://www.sqlite.org/download.html)
2. **Datenbank initialisieren**:  
   Führe das SQL-Skript `Library.sql` auf deiner SQLite-Datenbank aus.
3. **Projekt starten**:
   ```sh
   dotnet build
   dotnet run --project Library
   ```
4. **Im Browser öffnen**:  
   [http://localhost:5000](http://localhost:5000) (oder Port aus der Konsole)

---

## Entwicklung

- **Dependency Injection** für alle Services und Repositories
- **Razor Components** für wiederverwendbare UI-Elemente
- **CSS** für individuelles, modernes Styling
- **REST-API** für Mitarbeiterfunktionen (z.B. ausgeliehene Bücher)

---

## Hinweise

- **Passwörter** werden aktuell im Klartext gespeichert (nur zu Demo-Zwecken!)
- **Bilder** können als Base64-String im `Book`-Objekt gespeichert werden
- **Rollen**: `Customer` und `Employee` mit unterschiedlichen Rechten

---
