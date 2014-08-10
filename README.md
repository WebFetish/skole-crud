skole-crud
==========

Hvis du ikke er sikker på at du har lavet der rigtigt kan du se hvordan jeg har gjort det i Nyheder.aspx hvor jeg har lavet et eksempel.

Først skal du oprette en bruger, det gør du ved at åbne OprerBruger.aspx siden i din browser.

Først skal du oprette et Webform og du skal give den et navn. inden du opretter siden skal du være sikker på at du har valgt "Select master page".
Når du har trykket "add" skal du være sikker på at du trykker på admin mappen så du vælger den rigtige master page, husk også at du skal gemme selve siden i admin mappen.

Når du har oprettet siden går du ind i Template.aspx og kopierer alt inde i "Content2", og så kopierer du det ind i din nye aspx side. 
Nu kan du gå ind i din Template.aspx.cs og kopierer alt fra linje 5 og ned. Du skal så også overskrive alt fra linje 5 og ned på din egen .aspx.cs fil.
Du skal også lige tilføje "using System.Text;", og "using System.Data.SqlClient;" i toppen af din .aspx.cs side.

Hvis du prøver og trykke crtl + shift + b skulle der ikke være nogen fejl, hvis den mælder fejl har du lavet noget forkert.

Så skal du gøre så at den bliver vist i menuen, det gør du ved at gå til AdminMasterPage.master og der er en comment hvor du skal indsætte et menupunkt.

Nu skal du tilføje de forskellige texboxe som du skal bruge til rediger/opret siden. Dette gøres på .aspx siden hvor du kan se en som allerede er blever lavet.
Du skal bare kopierer der hvor der er markeret, og så lave det antal du skal bruge.
Husk og fjern/rediger den textbox som er der standard.

Nu skal du udfylde de tomme public strings som er i toppen.

Så skal du gå til linje 81, her skal du bruge builder.Append("<th></th>").AppendLine(); for hver "overskrift" som du skal bruge eks. Efternavn hvis skal have en list over folks efternavne.
På linje 91 skal du så tilføje samme antal som du har af overskrifter, inde i reader[""] skriver du det felt fra din db som skal loades.

På linje 164 skal du så tilføje hvad der loades ind i dine texboxe når du er på rediger siden. Husk og slet/rediger den nuværende som er lavet.

På linje 220 skal du ændre CommandText, du skal tilføje alle felter da dette er du opretter.

På linje 226 skal du også ændre CommandText, men her er det til når du redigere siden.

Nu skulle du gerne være færdig med at lave siden og alt skulle virke.

Nu ville jeg fjerne Nyheder.aspx og OpretBruger.aspx da du ikke får brug for dem mere.

