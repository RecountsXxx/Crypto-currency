This application uses libraries such as Newtonsoft.Json.
There are 3 pages of the application such as Dashboard - all coins, with their 
description.
Dashboard - Also by clicking on a coin there is a transition to a more detailed information about the coin, with a list of changes in the coin rate. 
detailed information about the coin, with a list of changes in the coin rate.
Exchange - there are 2 ComboBoxes with a choice of coins, which to exchange kollichestvo,
 and for which coin.
Settings - There is a choice of theme such as - dark theme and light theme, also
 there is a choice of application language such as English and Ukrainian.
Also present animation of the transition between pages.
The application uses the MVVM pattern.
Every 30 seconds, there is an update, so coins do not stand still, and
 can both rise and fall.
There is also a search by coin, by symbol, by rank, not taking into account the 
case, the search occurs immediately after entering a symbol, because it is made in the
 TextChanged event.
Used API of the exchange site - CoinCap.
Add a new language, or a new theme, you can very easily, you do not need to rewrite anything 
rewrite.
This application is created on the basis of a text task

Translated with DeepL.com (free version)
