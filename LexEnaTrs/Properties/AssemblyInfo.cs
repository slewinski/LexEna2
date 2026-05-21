using System;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("LexEnaTrs")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Energa.")]
[assembly: AssemblyProduct("LexEnaTrs")]
[assembly: AssemblyCopyright("Copyright © EIiT. 2026")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: CLSCompliant(true)]
[assembly: NeutralResourcesLanguage("pl-PL")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("bbe9c50a-5ef5-49d3-b9f6-c4039528e8e2")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Revision and Build Numbers 
// by using the '*' as shown below:
[assembly: AssemblyVersion("2.0.9.1")]
[assembly: AssemblyFileVersion("2.0.9.1")]
// 2.0.0.1 - wersja z importem 
// 2.0.0.2 - poprawka - odblokowanie konta 
// 2.0.0.3 Modyfikacja Z_BK_001191_12_2018 - raport potwierdzenia sald dla wniosków egzekucyjnych
// 2.0.1.1 Modyfikacja Importu do KRD 15/02/20120
// 2.0.2.1 Modyfikacja Zgony 25-02 + poprawka zamiana nr_domu na nr mieszkania
// 2.0.2.1 Modyfikacja Zgony 12-03 poprawka żaby nie brało z operatora.
// 2.0.2.2 Modyfikacja Zgony 30-03 dodanie oznaczenia bazy danych w oknie about
// 2.0.2.4 Doanie zestawienia _ należności w sprawie - narzędzia.
// 2.0.2.5 Importy postanowień o umorzeniu 
// 2.0.2.6 Zakładanie zleceń księgowych na podstawie zestawienai o zgonach 
// 2.0.2.7 Poprawiona wersja do importu dla EOP
// 2.0.2.8 Poprawioni import selen Pozaprądowy dal EOP
//2.0.2.9 - Import dokumentów zgonów. EOB
//2.0.2.10 - Poprawka importu xml spraw dow IWeny
//2.0.2.11 - Sortowanie i filtrowanie użytkowników, zestawienie wg dokumentu
//2.0.2.12 - Import Nakazów + zestawienie  zatorów
//2.0.2.13 - Import tytułów + poprawka import pozwów Alektum, poprawka vat o splacony,  
//2.0.3.1   - zmiana odsetek + import xml w nowym formacie
//2.0.3.2  - import z cc&B do KRD w nowym formacie 
// 2.0.4.1 - kapitalizacja odsetek + 40 EURO
// 2.0.5.1 - Masowa dekretacja spraw
// 2.0.5.2 - import umorzeń EPU
// 2.0.6.1 - Kryteria dodawania selenpzp do KRD
// 2.0.7.1 - Import danych KSeF do należności w Wienie.
// 2.0.8.1 - Dodanie KSEF, poprawka błędu - skrócenie 
// 2.0.9.1 - Nr ewid jako nr umowy