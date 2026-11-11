# Import spraw w paczkach, podział i dekretacja

## Wdrożenie

1. Wykonać kopię bezpieczeństwa baz LexEna i Wiena i wdrażać najpierw w środowisku testowym.
2. Uruchomić kolejno `001_BillingImportBatch.sql`, `002_Distribution.sql` i `003_DistributionApproval_LexEna.sql` w bazie wskazanej przez połączenie `LexEnaMeritumEntities`. Etap 002 dodaje mapowanie kancelarii, znacznik uczestnictwa oraz zapis propozycji podziału paczki. Etap 003 dodaje symbol kancelarii i stan zatwierdzenia paczki. Te trzy skrypty nie zmieniają bazy Wiena.
3. Uruchomić **`004_DistributionApproval_Wiena.sql` w bazie Wiena**, wskazanej przez `wiena_centralEntities`. Tworzy dziennik zatwierdzeń oraz unikalny indeks numeracji `sprawa (id_firmy, id_symbol, rok, poz)` dla dodatnich pozycji i lat z niepustym symbolem. Sprawdza wcześniej istniejące duplikaty, także w sprawach usuniętych. Jeśli je znajdzie, przerywa wdrożenie bez naprawiania danych; duplikaty trzeba wyjaśnić przed ponowieniem. Indeks obowiązuje również inne aplikacje zapisujące sprawy do Wieny. Sam skrypt nie przydziela kancelarii ani nie renumeruje spraw.
4. Skompilować solution `LexEnaTrs.sln` w Visual Studio 2015 (Silverlight 5 i WCF RIA Services). Zmiany EDMX, klasy encji i metadanych są dostarczone w repozytorium; nie trzeba aktualizować modelu z bazy.
5. Wdrożyć wspólnie serwer i wygenerowany klient Silverlight/XAP. Zachować istniejące połączenia do LexEna i Wieny. Sprawdzić na środowisku testowym uprawnienia konta aplikacji do odczytu/zapisu dziennika Wieny i do aktualizacji spraw.

Skrypty są dostarczone do wykonania przez osobę wdrażającą; nie są uruchamiane przez aplikację. Nie wybierają bazy poleceniem `USE` i można je uruchomić ponownie; nie przebudowują istniejących kolumn o innej strukturze. Tabela paczek i dziennik zatwierdzeń zawierają dane spraw w XML, więc wymagają takiej samej ochrony i polityki retencji jak pozostałe dane importowane do LexEna. `ImportXml` i `DistributionXml` są wyłączone z encji historii zwracanej przez RIA; szczegóły są odczytywane osobnymi operacjami.

Indeks filtrowany z etapu 004 wymaga właściwych ustawień sesji również przy zapisach ze starszego klienta Wieny: `ANSI_NULLS`, `QUOTED_IDENTIFIER`, `ANSI_PADDING`, `ANSI_WARNINGS`, `CONCAT_NULL_YIELDS_NULL`, `ARITHABORT` włączone, `NUMERIC_ROUNDABORT` wyłączone. Nowa ścieżka dekretacji ustawia je jawnie. Przed wdrożeniem produkcyjnym sprawdzić zwykłe dodawanie i edycję spraw ze wszystkich używanych klientów Wieny po instalacji skryptu; błędne ustawienia mogą blokować ich zapisy. Testy offline nie weryfikują konfiguracji tych aplikacji.

## Konfiguracja kancelarii

Kancelaria w LexEna jest rekordem `JednostkaWindykacji` typu „Zewnętrzna kancelaria prawna” (`TypJednostki = 1`). W istniejącym ekranie konfiguracji jednostek należy wybrać jej odpowiednik w Wienie oraz zaznaczyć „Uczestniczy w przydziale”. Pole `WienaFirmaId` przechowuje `firma.ident` z Wieny, a nie identyfikator jednostki LexEna. Lista wyboru korzysta z istniejącego słownika aktywnych kancelarii Wieny (`firma.typ = 9`, `aktywna = 1`) i pokazuje nazwę; ID wybranej kancelarii jest widoczne pod polem i na liście jednostek.

Domyślnie żadna jednostka nie uczestniczy w podziale. Powiązanie może pozostać puste, jeśli jednostka nie uczestniczy; pole wyboru pozwala je usunąć opcją „Brak powiązania”. Uczestnictwo wymaga prawidłowego powiązania z kancelarią Wieny. Kolumny listy pokazują udział, ID Wieny i symbol spraw.

Pole **Symbol spraw w Wienie** jest listą wyboru pobieraną bezpośrednio z tabeli `Wiena.symbol` dla aktywnych oddziałów bieżącej firmy. Lista pokazuje oznaczenie, np. `KA`, oraz nazwy oddziałów, w których występuje. Obejmuje również symbole, na których nie zarejestrowano jeszcze żadnej sprawy; nie zależy od rocznych liczników widoku importowego. Równoważne oznaczenia występują na liście raz, z informacją o ich oddziałach.

Konfiguracja nadal przechowuje oznaczenie w `SymbolSprawWiena`, o długości do 10 znaków. Zamiana pola tekstowego na listę wyboru nie wymaga nowego skryptu ani migracji istniejących oznaczeń. Identyfikatory `symbol.ident` zależą od oddziału, dlatego dekretacja odnajduje wybrane oznaczenie w **oddziale zapisanym przy paczce**. Spacje na początku i końcu oraz wielkość liter nie wpływają na dopasowanie. Brak symbolu w tym oddziale lub kilka pasujących rekordów blokują zatwierdzenie i są pokazane w tabeli kancelarii. Puste pole nie blokuje samej konfiguracji ani obliczenia propozycji, co pozwala zachować wcześniejsze ustawienia; trzeba wybrać symbol przed dekretacją. Zmiana symbolu lub powiązania kancelarii po obliczeniu propozycji wymaga ponownego wygenerowania podziału i sprawdzenia wyniku.

## Dwa przyciski zapisu

Wczytanie XML tworzy paczkę w LexEna. Przycisk **Zapisz wybór (LexEna)** zachowuje późniejsze zmiany zaznaczeń i odsetek, aby można było wrócić do paczki. Nie tworzy spraw w Wienie.

Przycisk **Utwórz sprawy w Wienie** zapisuje aktualnie wybrane sprawy na sygnaturze poczekalni i zamyka paczkę do edycji. Uwzględnia bieżący wybór, więc wcześniejsze kliknięcie zapisu wyboru nie jest konieczne.

Obie operacje wysyłają wyłącznie identyfikator pozycji, zaznaczenie i rodzaj odsetek. Dane odbiorców, należności i wpłat pozostają w zapisanym XML paczki po stronie serwera. Poprawka serializacji nie wymaga dodatkowego skryptu SQL.

## Usuwanie paczki

Przycisk **Usuń paczkę** przy historii usuwa otwartą paczkę z LexEna po potwierdzeniu. Jest dostępny tylko dla stanu „Przygotowana”, przed rozpoczęciem zapisu do Wieny. Usuwane są również XML i wybór spraw; źródłowy plik XML na komputerze pozostaje bez zmian. Po usunięciu znika wiersz historii i czyści się panel spraw. Funkcja nie wymaga zmiany schematu bazy.

Serwer niezależnie sprawdza status, liczbę zapisanych spraw oraz znaczniki zapisu w XML. Rozpoczęcie importu blokuje usunięcie nawet wtedy, gdy nie utrwalono jeszcze pierwszej sprawy. Równoczesna zmiana paczki lub rozpoczęcie zapisu do Wieny powoduje odrzucenie usunięcia.

## Scenariusze odbioru importu

Po kompilacji można uruchomić `Tests/ImportBillingPackages/Run-SmokeTests.ps1` z PowerShell. Testy korzystają z aktualnego `LexEnaTrs.Web/bin/LexEnaTrs.Web.dll`, nie otwierają połączeń z bazą i nie zapisują żadnych danych. Sprawdzają ochronę danych przed zmianami klienta, blokady duplikatów i zamkniętych paczek, scalanie należności, liczniki, kwoty, serializację oraz zgodność podstawowych elementów SQL/EDMX. Testy podziału obejmują wybór aktualnego statusu, odtworzenie propozycji po zmianie statusu lub mapowania, inicjalizację bez przypisań, walidację procentów oraz podział małych i dużych paczek. Testy dekretacji sprawdzają dopasowanie symboli, ograniczenia sygnatur, podgląd numeracji bez rezerwacji, zachowanie sygnatur z rejestracji XML, odrzucenie nieaktualnej lub niepełnej propozycji, ochronę już przeniesionych spraw i serializację potwierdzenia. Test historii weryfikuje też generowanie SQL z metadanych EDMX przy zamkniętym połączeniu, z fikcyjnym adresem serwera. Nie zastępują poniższych prób integracyjnych z testową Wieną, w szczególności prób współbieżności, wycofania transakcji i odzyskiwania po awarii.

- Otworzyć nową pozycję menu bezpośrednio pod istniejącym importem billingowym. Sprawdzić także działanie dotychczasowego importu.
- Wczytać poprawny XML, podać unikalne oznaczenie paczki, dokument, datę księgowania oraz sygnaturę poczekalni (oddział, symbol, numer początkowy, rok). Sprawdzić dane i liczbę spraw przed zapisem do Wieny.
- Zweryfikować obsługę braku wymaganych pól, błędnego lub pustego XML oraz powtórzonego oznaczenia paczki w tej samej firmie. Żaden z tych błędów nie powinien tworzyć nowych spraw w Wienie.
- Dla przygotowanej paczki sprawdzić anulowanie i potwierdzenie usunięcia, zniknięcie z historii oraz wyczyszczenie panelu. Paczki w trakcie zapisu, zapisane częściowo lub w całości i wymagające weryfikacji nie mogą być usuwane. W środowisku testowym sprawdzić równoczesną próbę usunięcia i rozpoczęcia importu — tylko jedna z tych operacji może się rozpocząć skutecznie.
- Zapisać paczkę do poczekalni, porównać sygnatury i liczbę zapisanych spraw z Wieną. Sprawy nie powinny być na tym etapie przydzielane kancelarii.
- Zamknąć ekran i otworzyć go ponownie. Odszukać paczkę w historii i sprawdzić jej oznaczenie, źródło, datę, liczniki i szczegóły spraw.
- Powtórzyć otwarcie zakończonej paczki i próbę zapisu; nie mogą powstać duplikaty spraw.
- W środowisku testowym sprawdzić częściowy błąd zapisu oraz utratę połączenia. Sprawy już zapisane nie mogą być zapisywane ponownie. Paczki o niepewnym wyniku wymagają weryfikacji z Wieną przed kolejną próbą.
- Sprawdzić, że historia nie udostępnia paczek innej firmy oraz że dwa równoległe zapisy tej samej paczki nie importują jej dwukrotnie.

Statusy techniczne: `Draft` — przygotowana paczka, `Processing` — zapis w toku, `Completed` — zakończona, `Partial` — częściowo zapisana, `ReviewRequired` — wynik wymagający sprawdzenia. Przed ręczną korektą statusu należy porównać zapisane sprawy i ich identyfikatory z Wieną.

## Scenariusze odbioru podziału

Przy pierwszym otwarciu okna **Podział na kancelarie** wszystkie skonfigurowane kancelarie są zaznaczone, a pola procentowe liczby i wartości spraw mają równe udziały. Otwarcie paczki, odświeżenie statusów i edycja udziałów nie wykonują podziału. Dopiero **Zaproponuj podział** odczytuje aktualne statusy, oblicza wynik i zapisuje propozycję w LexEna. Kolejne użycie tego przycisku zastępuje poprzednią propozycję wynikiem dla bieżących udziałów. Ponowne otwarcie pokazuje zapisane udziały i przypisania, pomijając sprawy, które przestały się kwalifikować; nie rozdziela ich automatycznie na nowo.

Każda kolumna procentowa ma sumować się do 100% wśród zaznaczonych kancelarii. Dopuszczalne są dwa miejsca po przecinku. Równe udziały, np. trzy razy 33,33%, są traktowane jako dokładnie równe i normalizowane do 100%. Przycisk **Ustaw równe udziały** ustawia obie kolumny dla aktualnie zaznaczonych kancelarii, bez uruchamiania podziału. Przy innych proporcjach liczba spraw jest zaokrąglana do całych spraw, a kwoty są przybliżane w ramach tych ograniczeń. Algorytm nie gwarantuje matematycznego optimum ani osiągnięcia niemożliwych proporcji, np. dodatniego udziału kwoty przy zerowej liczbie spraw.

Podczas automatycznego podziału sprawy znanych dłużników są przypisywane do kancelarii z najnowszej wcześniejszej sprawy w Wienie. Dopasowanie odbywa się w obrębie tej samej firmy, po `sprawa.nr_ewid` lub po PESEL/NIP dłużników połączonych przez `spr_dluz` z tabelą `skazani`. PESEL musi zawierać 11 cyfr, a NIP 10 cyfr (spacje i myślniki w NIP są pomijane). Puste i niepoprawne identyfikatory nie tworzą dopasowań. Wybierana jest najnowsza wcześniejsza, nieusunięta sprawa z przypisaną kancelarią: `d_kreacji DESC, ident DESC`. Sprawy z bieżącej paczki nie stanowią historii dla siebie nawzajem. Jawna ręczna zmiana opisana poniżej może wskazać inną kancelarię.

Tabela pokazuje **Poprzednią sygnaturę**, dotychczasową kancelarię i podstawę dopasowania. Dopiero **Zaproponuj podział** utrwala przypisania. Stała kancelaria ma pierwszeństwo przed udziałami procentowymi: nowa sprawa wnosi do jej bilansu jedną sprawę oraz swoją kwotę, a algorytm rozdziela pozostałe sprawy z uwzględnieniem tego obciążenia. Stałych przypisań nie wolno przenosić przy wyrównywaniu, również gdy już przekraczają docelowy udział kancelarii.

Kancelaria wymagana przez historię musi mieć aktywne powiązanie w konfiguracji i uczestniczyć w przydziale. Brak poprawnego powiązania blokuje wygenerowanie propozycji i jest opisany przy sprawie; wymaga poprawienia konfiguracji i odświeżenia widoku. Ponowne otwarcie i każde przeliczenie odczytują historię z Wieny. Ręczny wyjątek jest zachowywany tylko wtedy, gdy dane wcześniejszej sprawy i kancelarii nadal odpowiadają historii sprawdzonej podczas zmiany. Dane dopasowania są utrwalane w istniejącym XML propozycji; nie jest potrzebny nowy skrypt SQL.

- W konfiguracji oznaczyć uczestniczące kancelarie i przypisać ich odpowiedniki w Wienie. Sprawdzić zapis, ponowne otwarcie oraz możliwość usunięcia opcjonalnego powiązania. Próba włączenia jednostki wewnętrznej lub kancelarii bez mapowania powinna zostać odrzucona.
- Otworzyć paczkę bez zapisanej propozycji: wszystkie kancelarie zaznaczone, udziały równe, liczba przydzielonych spraw wynosi zero. Zmiana procentów również nie powinna tworzyć przypisań.
- Otworzyć zapisaną paczkę i pobrać aktualne dane spraw z Wieny. Do podziału kwalifikuje się wyłącznie aktualny status `85`; wszystkie pozostałe sprawy pozostają na liście jako wyszarzone i nie uczestniczą w obliczeniach.
- Zmienić status wybranej sprawy bezpośrednio w testowej Wienie i odświeżyć paczkę. Sprawdzić, że kwalifikacja wynika z aktualnego statusu, a nie ze stanu z dnia importu ani wyłącznie z oznaczenia zgonu.
- Kliknąć **Zaproponuj podział**, porównać liczby i kwoty z zadanymi udziałami. Zmienić udziały liczby i wartości niezależnie oraz ponownie kliknąć przycisk: powinien powstać nowy wynik. Niepoprawne udziały nie powinny zmieniać zapisanej propozycji. Wartość sprawy stanowi suma `Kwota_do_zaplaty` jej należności zapisanych w paczce; nie jest to bieżące saldo księgowe Wieny. Wiersze źródłowe scalone do jednej sprawy należy liczyć jako jedną sprawę.
- Dla znanego dłużnika sprawdzić dopasowanie po każdym identyfikatorze, ostatnią sygnaturę i kancelarię. Wprowadzić błędne długości PESEL/NIP w danych testowych — nie powinny łączyć dłużników. Dla kilku kancelarii w historii powinna wygrać najnowsza wcześniejsza sprawa. Zmienić procenty i ponowić podział: stałe przypisania pozostają, a ich kwoty i liczba uwzględniają się w sumach. Sprawdzić także przekroczenie udziału kancelarii przez same stałe przypisania.
- Zapisać propozycję, zamknąć ekran i ponownie otworzyć paczkę. Sprawdzić zachowanie wyniku oraz daty ostatniego zapisu propozycji.
- Potwierdzić w Wienie, że samo wygenerowanie i zapisanie propozycji nie zmieniło kancelarii, sygnatur ani numeracji spraw. Zapis do Wieny następuje dopiero po odrębnym zatwierdzeniu opisanym poniżej.

## Ręczna zmiana kancelarii przed zatwierdzeniem

W zapisanej propozycji należy użyć **Zmień kancelarię** w wierszu sprawy, wybrać docelową kancelarię i użyć **Zapisz zmianę kancelarii**. Pierwsza edytowalna sprawa jest zaznaczana po odczycie; można też wybierać inne wiersze. Starsza propozycja bez identyfikatora wersji wymaga jednorazowego użycia **Zaproponuj podział**, o czym ekran informuje wprost. Zmiana zostaje od razu zapisana w propozycji paczki w LexEna. Liczby spraw, kwoty i odchylenia kancelarii są przeliczane, ale pozostałe sprawy nie są ponownie rozdzielane. Zapis ręcznej zmiany nie modyfikuje jeszcze spraw, statusów ani sygnatur w Wienie.

Można wskazać każdą aktualnie skonfigurowaną kancelarię uczestniczącą w przydziale, również niezaznaczoną wcześniej do automatycznego podziału tej paczki. Taka kancelaria zostaje włączona do bilansu z docelowym udziałem 0% liczby i wartości; rzeczywisty bilans uwzględnia przeniesioną sprawę oraz jej kwotę. Przy równych udziałach zaokrąglonych, np. 3 × 33,33%, poprzednie udziały są korygowane wyłącznie o resztę zaokrąglenia do sumy 100%.

Jawny ręczny wybór może również zastąpić kancelarię wskazaną przez historię dłużnika. Poprzednia sygnatura i dane kancelarii z historii pozostają widoczne; kolumna **Ręcznie** oznacza wyjątek. Zapis obejmuje użytkownika i czas zmiany. Automatyczny podział nadal kieruje znanych dłużników do kancelarii z historii, a ręczny wyjątek jest respektowany przy odświeżaniu bilansu, ponownym otwarciu oraz zatwierdzaniu tej wersji propozycji.

Serwer ponownie sprawdza aktualny status 85, historię i mapowanie kancelarii. Nie można zmieniać spraw wyłączonych z podziału, bez przypisania, z nierozwiązanym powiązaniem historycznym, zadekretowanych ani paczek w trakcie zatwierdzania lub już zatwierdzonych. Zmiana historii lub konfiguracji od czasu zapisania propozycji wymaga jej ponownego sprawdzenia. Równoczesna edycja lub rozpoczęcie zatwierdzania w innej sesji blokuje zapis starej wersji.

Zmiana udziałów blokuje ręczną edycję do czasu zapisania nowej propozycji. Ponowne użycie **Zaproponuj podział** zastępuje ręczne zmiany nowym automatycznym wynikiem; ekran wymaga potwierdzenia, jeśli istnieją ręczne przypisania. Nie jest potrzebny dodatkowy skrypt SQL: zmiany są utrwalane w XML istniejącej propozycji.

Scenariusze odbioru w środowisku testowym:

- Przenieść pojedynczą sprawę i porównać przyrost/spadek liczby o 1 oraz dokładnej kwoty; pozostałe przypisania i procentowe cele mają pozostać bez zmian.
- Po odczycie propozycji sprawdzić zaznaczenie pierwszej edytowalnej sprawy i akcję w wierszu. Przewijanie tabeli kancelarii oraz odświeżanie bilansu nie mogą wyczyścić przypisań ani zablokować ręcznego wyboru. Rzeczywista edycja procentów ma wyczyścić lokalny podgląd i wymagać nowej propozycji. Sprawdzić osobno komunikat starszej propozycji oraz blokady stanów `Processing` i `Approved`.
- Ręcznie zmienić kancelarię sprawy znanego dłużnika; sprawdzić oznaczenie wyjątku, zachowanie danych historycznych i utrzymanie wyboru po odświeżeniu oraz ponownym otwarciu paczki.
- Wskazać skonfigurowaną kancelarię niezaznaczoną wcześniej do podziału; sprawdzić jej bilans i udział 0%, również przy trzech udziałach 33,33% lub sześciu 16,67%.
- Zmienić status lub historyczną kancelarię w Wienie między odczytem a zapisem; stara propozycja ma zostać odrzucona. Sprawdzić także niedostępne i zmienione mapowanie kancelarii docelowej.
- Ponownie zaproponować podział, anulować i zatwierdzić ostrzeżenie. Po zatwierdzeniu przeliczenia ręczne znaczniki znikają, a historia dłużnika znów określa automatyczną kancelarię.
- Zasymulować utratę odpowiedzi po ręcznym zapisie oraz równoczesną edycję lub zatwierdzanie w drugiej sesji. Po odświeżeniu ma być widoczna ostatnia zapisana wersja; nie wolno nadpisać jej starą propozycją.

## Zatwierdzenie podziału i dekretacja

Po sprawdzeniu zapisanej propozycji, kancelarii i ich symboli należy użyć **Zatwierdź i zadekretuj w Wienie**. Potwierdzenie pokazuje zakres operacji, oddział, rok numeracji i bilans kancelarii. Używany jest **rok zapisany przy paczce**, a nie automatycznie bieżący rok kalendarzowy. Zmiana udziałów blokuje zatwierdzenie do czasu ponownego użycia **Zaproponuj podział**. Wcześniejsze propozycje bez identyfikatora wersji wymagają jednorazowego ponownego przeliczenia. Samo otwarcie paczki, odświeżenie ani obliczenie propozycji nie wykonują dekretacji.

Kolumna **Sygnatura przed dekretacją** pokazuje sygnaturę nadaną podczas rejestracji XML do poczekalni, również przed przygotowaniem podziału. Kolumna **Sygnatura Wiena** pokazuje proponowaną sygnaturę docelową dla sprawy przypisanej do kancelarii. Podgląd uwzględnia symbol kancelarii, oddział, rok paczki i aktualną numerację w Wienie; odświeża się także po zapisaniu ręcznej zmiany kancelarii. Sprawy bez przydziału oraz wyłączone z podziału nie otrzymują proponowanej sygnatury. Błąd symbolu lub numeracji jest opisany przy sprawie.

Podgląd nie rezerwuje numerów. Jeśli przed zatwierdzeniem inny użytkownik zajmie następne numery, dekretacja nada kolejne wolne numery zgodnie z aktualnym stanem Wieny. Po zapisie **Sygnatura Wiena** pokazuje faktycznie nadaną sygnaturę, a **Sygnatura przed dekretacją** zachowuje wartość z rejestracji XML.

Serwer zatwierdza konkretną wersję zapisanej propozycji. Bezpośrednio przed zapisem ponownie odczytuje kwalifikację spraw, historię dłużników i mapowanie kancelarii. Sprawy muszą nadal być w poczekalni paczki i nie mieć już przypisanej kancelarii. Zmiana zbioru spraw o statusie 85, kwot, sygnatur, wymaganej kancelarii lub jej symbolu przerywa zatwierdzenie; trzeba odświeżyć dane i zaproponować podział ponownie. Nie ma automatycznego przeliczenia lub pomijania zmienionej sprawy podczas zatwierdzania. Sprawy od początku wyłączone z propozycji pozostają widoczne, lecz nie są dekretowane.

Dekretacja aktualizuje istniejące sprawy, zachowując ich identyfikatory: przypisuje `id_kancelarii`, docelowy `id_symbol`, `ksiega = symbol.typ`, numer `poz`, rok i sygnaturę w formacie `SYMBOL-numer/rok`. Statusy spraw pozostają bez zmian. Kolejny numer wynika z największego zajętego `poz` dla danego **oddziału, symbolu i roku**, także w rekordach oznaczonych jako usunięte. Nie są wypełniane wcześniejsze luki w numeracji. Sygnatura musi mieścić się w limicie 16 znaków Wieny; przekroczenie limitu lub zakresu numeracji przerywa zapis całej operacji.

Wszystkie zmiany spraw paczki oraz potwierdzenie w `dbo.LexEnaBillingDistributionApproval` są utrwalane w jednej transakcji Wieny. Dziennik ma unikalny identyfikator paczki i zapisuje wersję propozycji, użytkownika, czas oraz wynik ze starymi i nowymi sygnaturami. Blokady transakcji i unikalny indeks numeracji chronią przed przydzieleniem tego samego numeru równocześnie. Zatwierdzonej paczki nie można przeliczać ani ponownie renumerować.

Stan `DistributionState` jest niezależny od statusu importu: `Planning` — przygotowanie propozycji, `Processing` — rozpoczęte zatwierdzenie, `Approved` — potwierdzona dekretacja. Jeśli odpowiedź z serwera zaginie lub zapis wyniku w LexEna nie powiedzie się po zapisie w Wienie, należy odświeżyć paczkę i użyć **Sprawdź / dokończ dekretację**. Operacja odczytuje istniejący dziennik Wieny lub bezpiecznie kończy tę samą propozycję; nie nadaje numerów ponownie sprawom z już utrwalonym potwierdzeniem. Nie należy ręcznie cofać stanu `Processing` ani usuwać dziennika, bo służą rozpoznaniu już wykonanej operacji.

Po zatwierdzeniu tabela pokazuje zachowany wynik operacji. Kolumna wcześniejszej sygnatury dłużnika nadal dotyczy jego historycznej sprawy, która wskazała kancelarię dla automatycznego podziału. Ręczny wyjątek pozostaje oznaczony. Widok zatwierdzonej paczki jest zapisem wyniku dekretacji; nie aktualizuje historycznego podziału na podstawie późniejszych zmian w Wienie.

## Scenariusze odbioru dekretacji w środowisku testowym

- Wykonać skrypty 003 w LexEna i 004 w Wienie, następnie uruchomić je ponownie. W osobnej testowej bazie z duplikatem numeracji potwierdzić, że skrypt 004 zatrzymuje się bez zmiany spraw.
- Skonfigurować różne symbole kancelarii. Sprawdzić pusty, nieistniejący i niejednoznaczny symbol oraz symbol istniejący tylko w innym oddziale; żadna z tych sytuacji nie powinna pozwolić na zatwierdzenie. Oznaczenie prawidłowe w dwóch oddziałach powinno wskazać rekord z oddziału paczki.
- Zaproponować podział, sprawdzić bilans, a następnie anulować potwierdzenie. Żadne sygnatury ani kancelarie w Wienie nie powinny się zmienić. Ponowić i zatwierdzić: liczby oraz kwoty mają odpowiadać zaakceptowanej propozycji, z uwzględnieniem zapisanych ręcznych zmian, także wyjątków od kancelarii z historii dłużnika.
- Otworzyć nową paczkę i sprawdzić sygnatury z rejestracji XML w kolumnie **Sygnatura przed dekretacją**. Po propozycji sprawdzić docelowe sygnatury w kolumnie **Sygnatura Wiena**. Ręcznie zmienić kancelarię: podgląd ma przejść na jej symbol i numerację, a sygnatura z XML ma pozostać taka sama. Kolejne odświeżenia bez zmian w bazie nie mogą zajmować numerów.
- Między podglądem a zatwierdzeniem zarejestrować inną sprawę z docelowym symbolem i rokiem. Dekretacja ma użyć aktualnych kolejnych numerów, a tabela i potwierdzenie mają pokazać faktyczny wynik. Sygnatury pierwotnej rejestracji XML mają pozostać widoczne także po ponownym otwarciu wcześniej zatwierdzonej paczki.
- Sprawdzić ciągłość od największego numeru już występującego dla symbolu i roku, także jeśli największy rekord jest usunięty. Sprawdzić pusty rocznik, kilka kancelarii z tym samym symbolem oraz inną paczkę z innego oddziału lub roku. Numerów nie należy resetować przy kolejnej paczce.
- Między propozycją a zatwierdzeniem zmienić status jednej sprawy z 85, przypisać sprawę ręcznie do kancelarii albo zmienić mapowanie/symbol w konfiguracji. Zatwierdzenie powinno zostać odrzucone bez dekretacji pozostałych spraw. Powtórzyć dla zmiany najnowszej wcześniejszej sprawy znanego dłużnika.
- Wymusić błąd zapisu jednej sprawy, za długą docelową sygnaturę i kolizję numeru w testowej bazie. Sprawdzić, że transakcja nie pozostawia częściowo zmienionych spraw ani potwierdzenia zakończenia.
- Wykonać równoległe zatwierdzenie tej samej paczki w dwóch sesjach oraz dwóch różnych paczek używających tego samego symbolu i rocznika. Każda sprawa ma być zadekretowana tylko raz, bez powtórzonych numerów; dla błędu współbieżności można bezpiecznie ponowić tę samą operację.
- Zasymulować utratę odpowiedzi po zatwierdzeniu transakcji Wieny i błąd późniejszego zapisu wyniku do LexEna. Odświeżyć lub ponownie otworzyć paczkę, użyć sprawdzenia/dokończenia i porównać wynik z dziennikiem Wieny. Sygnatury i liczba wpisów dziennika nie mogą się zmienić przy ponowieniu.
- Po zatwierdzeniu ponownie otworzyć paczkę. Sprawdzić datę, użytkownika, stare i nowe sygnatury oraz zablokowane przyciski przeliczania i zatwierdzania. Statusy i wyłączone sprawy mają pozostać niezmienione.
