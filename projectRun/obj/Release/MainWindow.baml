   M S B A M L   `   `   `  ���� G  CprojectModel, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA0clr-namespace:projectModel;assembly=projectModelprojectModel   
projectRun   projectRun.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     IprojectModel0clr-namespace:projectModel;assembly=projectModel     -     �Name$  
mainWindow�� � Title$4 .Český pes - Coursingová závodní databáze��$	��520��$
��1000�� � WindowStartupLocation$ CenterScreen=� � WindowState$ 	Maximized=�$	��520��$
��1000�� � Icon$ 	ikona.ico�����	#FFEEE4C1 � 	Resources    
bigtextbox �� 
TargetType �� Property �� Value  smalltextbox  CZlabel  ENlabel��%�  &	        &	 �     &	 r    &	     �� " ��.���  � ��$ Center=���  � ��$ Center=���  � ��$ Center=���  � ��$ Center=���  � ��$ 70����  � ��$ 80����  � ��$
 14pt(��� " ��.���  � ��$ Center=���  � ��$ Center=���  � ��$ Center=���  � ��$ Center=���  � ��$ 30����  � ��$ 80����  � ��$
 12pt(��� " ��.���  � ��$ Center=���  � ��$ Center=���  � ��$ Center=���  � ��$ 30����  � ��$ 80����  � ��$
 11pt(��� " ��.���  � ��$ Center=���  � ��$ Center=���  � ��$ Center=���  � ��$ 30����  � ��$ 80����  � ��$	 8pt(�.��� $y�Visible=�	 � RowDefinitions	 �� $��25 ��� -   
 ��Name$

 row1��$��* �$��50���� $��15 ��� -   $

 row2��$��* �$��50���� $��20 �.+��� -    ��Name$ 	DGV_label��$y�	Collapsed=�$#��Hlavní databáze - MAJITELÉ��$��1��$��13(�$
��Left=�$	��Top=�$��0,-3,0,0q�&   System.Windows.Controls.DataGrid  -     Name$	 DGV��$y�	Collapsed=�$��12(�   
IsReadOnly . $��1��   HeadersVisibility$ Column=�   AutoGenerateColumns .  $��0,20,0,0q�   Columns 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn   Header$ ID��   MinWidth$ 30Q�1  +System.Windows.Controls.DataGridBoundColumn   Binding ��  �� Path$ id�  $ Jméno��$	 100Q� �� $ 	firstName�  $ 
Národnost��$ 50Q� �� $ 	narodnost�  $ Člen��$ 50Q� �� $
 clen�  $	 Psi�� �� $ 	pocet_psu�   ElementStyle �� " ��.���  � ��
 ��ح���  � ��
 �   � �� Triggers P�  P� Property � �� P� Value$ 1��. ���  � ��
 ����   $	 Kč��$ 80Q� �� $ penize�  $ Platba��$ 50Q� �� $ 	potvrzeni�  $ Telefon��$ 90Q� �� $ telefon�  $ Email��$	 200Q� �� $ email� � ContextMenu �� .@�m� -   $��Nový majitel��m� -   $��Upravit majitele���� m� -   $��Přidat psa���� m� -	   $��Smazat majitele���� -
    ��Name$ stat_p��$y�	Collapsed=����	#FFACACAC$��1��$��1��$��1��.+��� $��Statistika psů��$
��12pt(�$
��Bold"�
 �� Top$ 10�� �� Left$ 10���� $��Počet psů��$ 40��$ 10���� $��Psi do osmi let věku��$ 63��$ 10���� $��Psi nad osm let věku��$ 86��$ 10���� $��Osmiletí psi��$	 109��$ 10���� $&�� Psi zapsaní pro licenční běh��$	 132��$ 10���� -    ��Name$ hide_stat_p��$��OK��$	 170��$ 10��$��23��$��80���� -    ��Name$ splitter��$y�	Collapsed=�$��Stretch=�$��1��$��2�� J� OpacityMask
 �   ����	#FFEEE4C1�� -   $ DGV_P_label��$y�	Collapsed=�$��Hlavní databáze - PSI��$��3��$��13(�$
��Left=�$	��Top=�$��0,-3,0,0q�  -   $ DGV_P��$y�	Collapsed=�$��12(� . $��3��$ Column=� .  $��0,20,0,0q�   $ Číslo psa�� �� $ 	StartBeh1�  $ Jméno��$	 200Q� �� $ Jmeno�  $ Plemeno��$	 150Q� �� $ Plemeno�  $ Pohlaví�� �� $ Pohlavi�  $ 	Narození�� �� $ Datum�  �� StringFormat$  d.M.yyyy��  $	 Z-L��$ 50Q� �� $ ZavodLicence� �� " ��.���  � ��
 �   � P�  � ��$ Licence��. ���  � ��
 �����P�  � ��$ Závod��. ���  � ��
 ����P�  � ��$ Tréning��. ���  � ��
 ��ح�  $ Licence�� �� $ Licence�  $ Majitel�� �� $ MajitelJmeno�  $ Platba�� �� $ Platba�  $ Doplatit�� �� $ Doplatit�  $ Skupina�� �� $ Poznamka� �� .@�m� -   $��Upravit psa���� �� m� -   $��Seřadit k prvnímu běhu��m� -   $��Seřadit k druhému běhu���� m� -   $��
Smazat psa���� -   $ stat_m��$y�	Collapsed=����	#FFACACAC$��1��$��3��$��1��.+��� $��Statistika majitelů��$
��12pt(�$
��Bold"�$ 10��$ 10���� $��Počet členů��$ 40��$ 10���� $��Počet nečlenů��$ 63��$ 10���� $��Již zaplatili��$ 86��$ 10���� $��Ještě nezaplatili��$	 109��$ 10���� -   $ hide_stat_m��$��OK��$	 170��$ 10��$��23��$��80���� -   $ skupGrid_label��$y�	Collapsed=�$��Přehled psů��$��1��$��13(�$	��Top=�$
��Left=�$��0,-3,0,0q�  -   $ 
skupinGrid��$y�	Collapsed=�$ Column=�$��12(�$ 5��$ 5��$��1��$��3�� .  .  $��0,20,0,0q�����    $
 Beh1��!   Width5  /System.Windows.Controls.DataGridLengthConverter$! 30  �� $ 	StartBeh1�  $ Dvojice1��$! 30  �� $ Dvojice0�  $ Barva1�� �� $ Barva0�  $
 Beh2��$! 30  �� $ 	StartBeh2�  $ Dvojice2��$! 30  �� $ Dvojice1�  $ Barva2�� �� $ Barva1�  $ Umístění��$! 30  �� $ Skupina�  $ Jméno��$	 200Q� �� $ Jmeno�  $ Plemeno��$	 150Q� �� $ Plemeno�  $	 Z-L��$ 50Q� �� $ ZavodLicence� �� " ��.���  � ��
 �   � P�  � ��$ Licence��. ���  � ��
 �����P�  � ��$ Závod��. ���  � ��
 ����P�  � ��$ Tréning��. ���  � ��
 ��ح�  $ Majitel��$	! 150  �� $ MajitelJmeno�  $ 	Obratnost��$! 25  �� $ 	AgilityA0�  $ Rychlost��$! 25  �� $ SpeedA0�  $ 
Vytrvalost��$! 25  �� $ EnduranceA0�  $ 	Nadšení��$! 25  �� $ EnthusiasmA0�  $ Inteligence��$! 25  �� $ IntelligenceA0�    $ 	Obratnost��$! 25  �� $ 	AgilityA1�  $ Rychlost��$! 25  �� $ SpeedA1�  $ 
Vytrvalost��$! 25  �� $ EnduranceA1�  $ 	Nadšení��$! 25  �� $ EnthusiasmA1�  $ Inteligence��$! 25  �� $ IntelligenceA1�    $ Body1�� �� $ Body1�    $ 	Obratnost��$! 25  �� $ 	AgilityB0�  $ Rychlost��$! 25  �� $ SpeedB0�  $ 
Vytrvalost��$! 25  �� $ EnduranceB0�  $ 	Nadšení��$! 25  �� $ EnthusiasmB0�  $ Inteligence��$! 25  �� $ IntelligenceB0�    $ 	Obratnost��$! 25  �� $ 	AgilityB1�  $ Rychlost��$! 25  �� $ SpeedB1�  $ 
Vytrvalost��$! 25  �� $ EnduranceB1�  $ 	Nadšení��$! 25  �� $ EnthusiasmB1�  $ Inteligence��$! 25  �� $ IntelligenceB1�    $ Body2�� �� $ Body2�  $ Diskvalifikace��$! 50  �� $ Diskval� �� " �� P�  � ��$	 DNS��. ���  � ��
 �  ��P�  � ��$	 DNF��. ���  � ��
 �  ��P�  � ��$ N��. ���  � ��
 �  �� �� .@�m� -   $��Upravit psa���� m� $��Prohodit dva psy��.@�m� -   $��První běh��m� -   $��Druhý běh���� m� -   $��Hodnotit první kolo psa��m� -   $��Hodnotit druhé kolo psa��m� -   $��Finish��� -   " �Name$" hodnoceniGrid��$��1��$��4��$��0,20,0,0q�$y�Visible=�.+��� -   # ��Name$# hodnoceni_rect��$��Center=�$��0,0,0,20q�$	��353�����	#FF515151$��4��$��Center=�$	��811������ $ �� Color$$ 	#FF78A0C8���� -   % ��Name$% hod_kolo��$��16(�$��40��$	��100��$��0,0,350,280q�& �� SelectedIndex$& 0��' �� 
IsReadOnly' . .@��� $��1. kolo���� $��2. kolo���� $��Diskvalifikace��$��30��$
��11pt(�$��Center=�$��Center=�$��0,120,450,0q�$	��100��$��Center=��� $��Disqualification��$��30��$	��8pt(�$��Center=�$��Center=�$��0,160,450,0q�$��80��$��Center=��� -    $% hod_dis��$��14(�$��30��$	��100��$��0,210,440,0q�$& 0��' . .@��� $	��---���� $	��DNF���� $	��DNS���� $��N���� $��
Bodování��$
��16pt(�$��Center=�$��Center=�$��0,0,600,280q��� $��Č. psa��#��[ $��0,0,600,140q��� $��Dog No.��#��[ $��
0,0,600,90q��� $��	Obratnost��#��[ $��0,0,200,140q��� $��Agility��#��[ $��
0,0,200,90q��� $��Rychlost��#��[ $��	0,0,0,140q��� $��Speed��#��[ $��0,0,0,90q��� $��
Vytrvalost��#��[ $��200,0,0,140q��� $��	Endurance��#��[ $��
200,0,0,90q��� $��	Nadšení��#��[ $��400,0,0,140q��� $��
Enthusiasm��#��[ $��
400,0,0,90q��� $��Inteligence��#��[ $��600,0,0,140q��� $��Intelligence��#��[ $��
600,0,0,90q��� $��Součet��$��30��$
��12pt(�$��Center=�$��Center=�$��500,0,0,140q�$��80��$��Center=�$y�Hidden=��� $	��Sum��$��30��$	��8pt(�$��Center=�$��Center=�$��
500,0,0,90q�$��80��$��Center=�$y�Hidden=��� $��Celkový součet��$��30��$
��12pt(�$��Center=�$��Center=�$��700,0,0,140q�$	��120��$��Center=�$y�Hidden=��� $��	Total sum��$��30��$	��8pt(�$��Center=�$��Center=�$��
700,0,0,90q�$��80��$��Center=�$y�Hidden=��� -!   ( ��Name$( hod_id��#��[  $��156,186,756,192q��� -"   $( 
hod_agil_A��#��[ $��
0,0,200,40q��� -#   $( hod_speed_A��#��[ $��0,0,0,40q��� -$   $( hod_endur_A��#��[ $��
200,0,0,40q��� -%   $( hod_enthu_A��#��[ $��
400,0,0,40q��� -&   $( hod_intel_A��#��[ $��
600,0,0,40q��� -'   $( 
hod_agil_B��#��[ $��
0,40,200,0q��� -(   $( hod_speed_B��#��[ $��0,40,0,0q��� -)   $( hod_endur_B��#��[ $��
200,40,0,0q��� -*   $( hod_enthu_B��#��[ $��
400,40,0,0q��� -+   $( hod_intel_B��#��[ $��
600,40,0,0q��� $��Rozhodčí č. 1��$��30��$	��8pt(�$��Center=�$��Center=�$��
0,0,400,40q�$��80��$��Center=�$��Center=��� ���	#FF515151$��	110,0,0,0q�$��4��	) �� X1$) 0��	* �� X2$	* 700��	+ �� Y1$+ 0��	, �� Y2$, 0��$��Center=�$��Center=�$	��585���� $��Rozhodčí č. 2��$��30��$	��8pt(�$��Center=�$��Center=�$��
0,40,400,0q�$��80��$��Center=�$��Center=��� -,   $��OK��$
��14pt(�$��40��$	��100��$��	0,200,0,0q�- � ToolTip$ - Uloží body do databáze.���� --   $��storno��$
��11pt(�$��28��$��90��$��650,250,0,0q�$ - Uloží body do databáze.���� -.   $( 	hod_sum_A��$��30��$
��12pt(�$��Center=�$��Center=�$��
500,0,0,40q�$��80��$��Center=�$��Center=�. �� 
IsReadOnly. . $y�Hidden=��� -/   $( 	hod_sum_B��$��30��$
��12pt(�$��Center=�$��Center=�$��
500,40,0,0q�$��80��$��Center=�$��Center=�. . $y�Hidden=��� -0   $( hod_sum_TOTAL��$��70��$
��14pt(�$��Center=�$��Center=�$��	700,0,0,0q�$��80��$��Center=�$��Center=�. . $y�Hidden=��� -1   $ divNewMajitel��$��1�����	#FF919191$y�	Collapsed=�$	��173��$	��Top=�$��Right=�$	��850��.+��� -2   $( boxId��$y�	Collapsed=��� -3   $ jmenoLBL��$��Jméno��$ 10��$��23��$ 10���� -4   $( boxFirstName��$��23��$	 113��/ �� TextWrapping$
/ Wrap=�$ 10��$	��300���� -5   $ prijmeniLBL��$��Příjmení��$ 38��$ 10��$y�	Collapsed=��� -6   $( boxLastName��$��23��$	 113��$
/ Wrap=�$ 38��$	��300��$y�	Collapsed=��� -7   $ clenLBL��$��
Členství��$ 66��$ 10���� -8   0 ��Name$0 clenCB��$	 113��$ 71���� -9   $ 	platbaLBL��$��Platba��$ 66��$	 206���� -:   $( platbaTB��$��23��$	 251��$
/ Wrap=�$ 66��$��99��$��0���� -;   $ psiLBL��$��Počet psů��$ 66��$	 506���� -<   $( psiTB��$��23��$	 581��$
/ Wrap=�$ 66��$��25���� -=   $ plat_OK_LBL��$��	Zaplaceno��$ 66��$	 387���� ->   $0 platba_OK_CB��$	 451��$ 71���� -?   $ 
telefonLBL��$��Telefon��$ 38��$	 449���� -@   $( 
telefonBox��$��23��$	 540��$ 38��$	��300���� -A   $ emailLBL��$��Email��$ 17��$	 449���� -B   $( emailBox��$��23��$	 540��$ 10��$	��300���� $��
Národnost��$ 66��$	 650���� -C   $( boxNarod��$��23��$	 740��$
/ Wrap=�$ 66��$	��100���� -D   $��Uložit��$ 10��1 �� Bottom$1 10��$��75���� -E   $��Zrušit��$ 90��$1 10��$��75���� -F   $ 	divNewPes�����	#FF919191$��2��$y�	Collapsed=�$��1��$	��170��$��Bottom=�$��Right=�$	��850��$��2��.+��� -G   $ 
cisloLBL_P��$��Č. psa��$ 10��$��23��$ 10���� -H   $( boxId_P��$��23��$ 80��$
/ Wrap=�$ 10��$��50��$y�	Collapsed=��� -I   $( 
boxstart_P��$��23��$ 80��$
/ Wrap=�$ 10��$��50���� -J   $ cislobehuLBL_P��$��	Č. běhu��$ 10��$	 150���� -K   $( boxBeh��$��23��$	 210��$
/ Wrap=�$ 10��$��50���� -L   $ 
jmenoLBL_P��$��Jméno��$ 38��$��23��$ 10���� -M   $( boxFirstName_P��$��23��$ 80��$
/ Wrap=�$ 38��$	��180���� -N   $ plemenoLBL_P��$��Plemeno��$ 38��$	 293���� -O   $% comboPlemeno��$��23��$	 392��$ 38��$	��150���� -P   $ pohlaviLBL_P��$��Pohlaví��$ 66��$ 10���� -Q   $ fciLBL��$	��FCI��$ 10��$	 598��$y�	Collapsed=��� -R   $% fciCombo��$��23��$	��150��$	 690��$ 10��$y�	Collapsed=��� -S   $ datumLBL��$��Datum narození��$ 38��$	 598���� $��Licence��$ 66��$	 293���� -T   $( 
boxLicence��$��23��$	 392��$
/ Wrap=�$ 66��$	��150��(  "System.Windows.Controls.DatePicker  -U   2  Name$2 datumPicker��$	 690��$ 38��$	��150��3   FirstDayOfWeek$3 Monday=�4   SelectedDateFormat$4 Short=��� -V   $% 	pohlaviCB��$
��Left=�$	��Top=�$��85��$ 80��$ 66��$& 0��' . $5 �� IsSynchronizedWithCurrentItem$
5 TrueY�$��23��.@��� $	��pes�����	#FF00B9FF�� $
��fena�����	#FFFF6A6A�� $��Z/L/T��$ 94��$ 10���� -W   $% zavodCB��$& 0��$ 80��$ 94��$��85��$��23��.@��� $��Závod���� $��Licence���� $��Tréning���� $��Majitel (ID)��$ 94��$	 293���� -X   $( 
boxMajitel��$��23��$	 392��$
/ Wrap=�$ 94��$	��150���� $��Platba/doplatek��$ 66��$	 598���� -Y   $( boxPlat��$��23��$	 690��$
/ Wrap=�$ 66��$��70��$��0���� -Z   $( 	boxDoplat��$��23��$	 770��$
/ Wrap=�$ 66��$��70��$��0���� $��Skupina��$ 10��$	 293���� -[   $( boxPozn��$��23��$	 392��$
/ Wrap=�$ 10��$��50���� -\   $��Uložit��$ 10��$1 10��$��75��6 �� 	IsDefault6 . �� -]   $��Zrušit��$ 90��$1 10��$��75���� -^   $ diveditprehled�����	#FF919191$��3��$y�	Collapsed=�$	��100��$��Bottom=�.+��� -_   $ 	start1LBL��$��Č. psa��$ 10��$ 10���� -`   $( 	boxstart1��$��23��$ 60��$
/ Wrap=�$ 10��$��50���� -a   $ beh1LBL��$��	Č. běhu��$ 10��$��23��$	 130���� -b   $( boxbeh1��$��23��$	 180��$
/ Wrap=�$ 10��$��50���� $��Dečka��$ 10��$	 250���� -c   $% decka1CB��$& 0��$	 290��$ 10��$��85��$��23��.@��� $��	červená���� $��bílá���� -d   $ 	start2LBL��$��Č. psa��$ 10��$	 400���� -e   $( 	boxstart2��$��23��$	 450��$
/ Wrap=�$ 10��$��50���� -f   $ beh2LBL��$��	Č. běhu��$ 10��$��23��$	 520���� -g   $( boxbeh2��$��23��$	 570��$
/ Wrap=�$ 10��$��50���� $��Dečka��$ 10��$	 650���� -h   $% decka2CB��$& 0��$	 690��$ 10��$��85��$��23��.@��� $��	červená���� $��bílá���� -i   $��Uložit��$ 10��$1 10��$��75��6 . �� -j   $��Zrušit��$ 90��$1 10��$��75��o� $��0��$��2��.@�m� $
��Menu��$��15(�$=- 7Hlavní nabídka, načítání a ukládání databáze.��.@�m� $��Nový��$- Mimo provoz!��
��������� m� $��Načíst��.@�m� -k   $��Načíst EXCEL��$-- 'Načte novou databázi z Excel tabulek.��m� -l   $��Načíst XML��$/- )Načte databázi uloženou v XML souboru.��m� -m   $��Uložit��$2- ,Uloží aktuální databázi do XML souboru.���� m� -n   $��Ukončit��$#- Bezpečně ukončí aplikaci.��m� $��Funkce��$��15(�$%- Funkce pro práci s databází.��.@�m� -o   $��Hlavní databáze��$2- ,Zobrazí hlavní databázi majitelů a psů.���� m� -p   $��Přehled psů��$1- +Zobrazí přehled psů a stav jejich bodů.���� m� -q   $��
Hodnocení��$- Spustí modul bodování.���� m� $��Uložit rozpis do Excelu��$s- mZapíše rozpis prvního běhu do tabulky Excel. Pokud neni tabulka načtena, dotáže se na cílový soubor.��.@�m� -r   $��
Běh č. 1��m� -s   $��
Běh č. 2��m� $.��(Uložit tabulky pro rozhodčí do Excelu��.@�m� -t   $��
Běh č. 1��m� -u   $��
Běh č. 2��m� $!��Uložit výsledky do Excelu��.@�m� -v   $)��#Běh č. 1 (průběžné výsledky)��m� -w   $%��Běh č. 2 (celkové výsledky)��m� $
��Help��$��15(�$(- "Nápověda a informace o programu.��.@�m� $��Testovací funkce��$O- IPovolí funkce pro testování, např. náhodné generování výsledků.��$y�Visible=�.@�m� -x   $"��Náhodné body kolo 1 (TEST)��m� -y   $"��Náhodné body kolo 2 (TEST)��m� -z   $��
O programu��$+- %Informace o programu a jeho autorovi.���� -{   $ 
help_about�����	#FF347299$y�	Collapsed=�$	��240��$��Center=�$��Center=�$	��420��$��0,0,0,0q�$��0��$��6��.+��� $��Images/logo_w.png��$��Center=�$ 10��$��60,0,0,0q��� $��14(�
�������$��18��$	��420��$	 127��$��WrapWithOverflow=�7 �� TextAlignment$7 Center=�$��Center=�GEDatabázový systém pro správu a řízení coursingových závodů.�� 
�������$��Center=�$��15��$	��420��$	 150��$��WrapWithOverflow=�$7 Center=�Bc. Jiří Růžička�� 
�������$��Center=�$��20��$	��420��$	 170��$��WrapWithOverflow=�$7 Center=�FEKT VUT v Brně 2015�� -|   $��Center=�$��OK��$1 10��$��80��$	 210��$	 170��]� $��6��.+��� $��Bottom=����� .R�� 	 �� $��* �8 � ColumnDefinitions8 �� $��4* ��� $
��Auto ��� $��* ��� $��* �.@��� $��0��.���� -}   9 ��Name$9 statusBlock��$C��=Počet ohodnocených psů: 0, počet zbývajících psů:  0.���� $��1��.���� -~   $9 statusHodnoceniBlock��$C��=Počet majitelů v databázi: 0, počet psů v databázi:  0.���� .��Main Content