skole-crud
==========

Hvis du ikke er sikker p� at du har lavet der rigtigt kan du se hvordan jeg har gjort det i Nyheder.aspx hvor jeg har lavet et eksempel.

F�rst skal du oprette en bruger, det g�r du ved at �bne OprerBruger.aspx siden i din browser.

F�rst skal du oprette et Webform og du skal give den et navn. inden du opretter siden skal du v�re sikker p� at du har valgt "Select master page".
N�r du har trykket "add" skal du v�re sikker p� at du trykker p� admin mappen s� du v�lger den rigtige master page, husk ogs� at du skal gemme selve siden i admin mappen.

N�r du har oprettet siden g�r du ind i Template.aspx og kopierer alt inde i "Content2", og s� kopierer du det ind i din nye aspx side. 
Nu kan du g� ind i din Template.aspx.cs og kopierer alt fra linje 5 og ned. Du skal s� ogs� overskrive alt fra linje 5 og ned p� din egen .aspx.cs fil.
Du skal ogs� lige tilf�je "using System.Text;", og "using System.Data.SqlClient;" i toppen af din .aspx.cs side.

Hvis du pr�ver og trykke crtl + shift + b skulle der ikke v�re nogen fejl, hvis den m�lder fejl har du lavet noget forkert.

S� skal du g�re s� at den bliver vist i menuen, det g�r du ved at g� til AdminMasterPage.master og der er en comment hvor du skal inds�tte et menupunkt.

Nu skal du tilf�je de forskellige texboxe som du skal bruge til rediger/opret siden. Dette g�res p� .aspx siden hvor du kan se en som allerede er blever lavet.
Du skal bare kopierer der hvor der er markeret, og s� lave det antal du skal bruge.
Husk og fjern/rediger den textbox som er der standard.

Nu skal du udfylde de tomme public strings som er i toppen.

S� skal du g� til linje 81, her skal du bruge builder.Append("<th></th>").AppendLine(); for hver "overskrift" som du skal bruge eks. Efternavn hvis skal have en list over folks efternavne.
P� linje 91 skal du s� tilf�je samme antal som du har af overskrifter, inde i reader[""] skriver du det felt fra din db som skal loades.

P� linje 164 skal du s� tilf�je hvad der loades ind i dine texboxe n�r du er p� rediger siden. Husk og slet/rediger den nuv�rende som er lavet.

P� linje 220 skal du �ndre CommandText, du skal tilf�je alle felter da dette er du opretter.

P� linje 226 skal du ogs� �ndre CommandText, men her er det til n�r du redigere siden.

Nu skulle du gerne v�re f�rdig med at lave siden og alt skulle virke.

Nu ville jeg fjerne Nyheder.aspx og OpretBruger.aspx da du ikke f�r brug for dem mere.

