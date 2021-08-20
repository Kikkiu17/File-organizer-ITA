import shutil                                                                                               #importa shutil e os 
import os
from os.path import dirname, abspath                                                                        #importa cose varie
firma = "################################################################\n###   ####   ###  ###   ####   ###   ####   ###  ###  #####  ###\n###   ###   ####  ###   ###   ####   ###   ####  ###  #####  ###\n###   ##   #####  ###   ##   #####   ##   #####  ###  #####  ###\n###       ######  ###       ######       ######  ###  #####  ###\n###   ##   #####  ###   ##   #####   ##   #####  ###  #####  ###\n###   ###   ####  ###   ###   ####   ###   ####  ###  #####  ###\n###   ###   ####  ###   ###   ####   ###   ####  ###         ###\n#################################################versione:1.1.0#"
print(firma)
direc = os.path.dirname(os.getcwd())+"\\"                                                                   #dichiara la parent directory della directory attuale
assoluta = dirname(abspath(__file__))                                                                       #dichiara la directory attuale (quella in cui c'è il file)
lines = ""                                                                                                  #variabile per il file di config
with open(assoluta+"\\config.txt") as f:                                                                    #apre il file di config
    lines = f.readlines()                                                                                   #legge il file
linnn = ''.join(lines).split("\n")                                                                          #crea un array del file
x = os.listdir(direc)                                                                                       #la cartella in cui si deve effettuare il riordinamento
c = 0                                                                                                       #variabile per controllare se è cambiato qualcosa
lin = ''.join(linnn[1]).split(":")                                                                          #dichiara 2 liste, con il contenuto del file di config
lvn = ''.join(linnn[1]).split(":")
h = 0                                                                                                       #variabile per la creazione della cartella primaria
C = 0
hh = ""

for g in x:                                                                                                 #ciclo for per controllare se è presente la directory primaria
    if(lin[0] == "dir"):
        if(g == lin[1]):
            h=h+1                                                                                           #se c'è, aggiunge 1 ad "h"
gg=0
if(h == 0):                                                                                                 #se "h" è 0, quindi non c'è la directory principale, la crea col nome impostato nel file di config
    with open(assoluta+"\\data.txt") as fil:                                                                #apre il file di dati
        dati = fil.readlines()                                                                              #legge il file
    A = ''.join(dati).split("\n")                                                                           #mette il contenuto separato ogni linea a capo in una variabile
    pdir = ''.join(A[1]).split(",")                                                                         #prende la seconda linea nel file di dati
    if(pdir[1] != assoluta):                                                                                #controlla se la directory nel file di dati è uguale a quella in cui si trova attualmente lo script
        directo = open("data.txt", "w")                                                                     #in questo caso apre il file
        n = directo.write("name,null\n"+"pdir,"+assoluta)                                                   #e reimposta i valori, togliendo il nome della directory principale e sovrascrivendo la vecchia directory con quella attuale
        directo.close()
    with open(assoluta+"\\data.txt") as fil:                                                                #riapre il file di dati dopo le modifiche
        dat = fil.readlines()                                                                               #legge il file
    B = ''.join(dat).split("\n")                                                                            #mette il contenuto separato ogni linea a capo in una variabile
    testdata = ''.join(B[0]).split(",")                                                                     #mette il contenuto separato ogni due punti a partire dalla prima linea in una variabile
    if(testdata[1] == "null"):                                                                              #se la directory principale non è presente,
        os.mkdir(direc+lin[1])                                                                              #ne crea una
        print(">>> Benvenuto <<<\n>>> Directory creata:",direc+lin[1],"<<<")                                #scrive che la directory principale è stata creata
        text_file = open("data.txt", "w")                                                                   #apre il file di dati
        n = text_file.write("name,"+lin[1]+"\n"+"pdir,"+assoluta)                                           #scrive il nome della directory principale nel file
        text_file.close()
        gg=gg+1                                                                                             #aggiunge 1 alla variabile in modo che si possa vedere che è stata creata la directory
    else:                                                                                                   #se la directory è già esistente il nome di essa è noto,
        os.rename(direc+testdata[1], direc+lin[1])                                                          #rinomina la directory principale col nuovo nome
        print(">>> Directory rinominata da",testdata[1],"a",lin[1],"<<<")                                   #scrive che è stata rinominata la directory
        text_file = open("data.txt", "w")                                                                   #apre il file di dati
        n = text_file.write("name,"+lin[1]+"\n"+"pdir,"+assoluta)                                           #aggiorna il nome della directory principale nel file di dati
        text_file.close()
        gg=gg+1

j = os.listdir(direc+lin[1])                                                                                #variabile per il nome delle directory secondarie nel file
k = 3                                                                                                       #variabile per ciclo for
m = 0                                                                                                       #variabile per controllo nel ciclo for
t = ""                                                                                                      #variabile per conservare le directory secondarie da creare
for i in linnn:                                                                                             #ciclo for per controllare quali directory secondarie mancano, altrimenti se ci sono tutte non fa niente
    if(k <= len(linnn)):                                                                                    #avvia il contenuto solo se "k" è minore della lungezza di "linnn", in modo da non uscire dalla lunghezza della lista
        lun = ''.join(linnn[k-1]).split(":")                                                                #crea una variabile con il nome della directory attuale, va dall'alto al basso nel file di config
        for n in j:                                                                                         #ciclo for per controllare le directory secondarie presenti nella directory principale
            if(n == lun[len(lun)-1]):                                                                       #se la directory secondaria selezionata è uguale (quindi esiste) alla directory selezionata nel file di config,
                m=m+1                                                                                       #aggiunge 1 a "m" per la verifica
        if(m > 0):                                                                                          #quindi se è presente,
            m=0                                                                                             #reimposta "m" a 0, nessuna azione necessaria
        elif(m == 0):                                                                                       #altrimenti, se è 0 quindi non c'è,
            if(t == ""):                                                                                    #e se non c'è niente nella variabile t,
                t = t+lun[len(lun)-1]                                                                       #scrive in quella variabile la directory da creare senza il ritorno a capo
            else:
                t = t+":"+lun[len(lun)-1]                                                                   #altrimenti mette il ritorno a capo se c'è qualcosa nella variabile
        k=k+1                                                                                               #aumenta di 1 il counter ad ogni fine loop

v = ''.join(t).split(":")                                                                                   #divide la variabile "t" (con le directory secondarie da creare) ogni due punti
if(t != ""):                                                                                                #se c'è qualcosa all'interno della variabile "t", quindi ci sono directory secondarie da creare, le crea
    for s in v:                                                                                             #ciclo for per operare su ogni directory presente nella variabile
        os.mkdir(direc+lin[1]+"\\"+s)                                                                       #crea la directory
        print("Directory creata:",direc+lin[1]+"\\"+s)
        gg=gg+1                                                                                             #aggiunge 1 alla variabile in modo che si possa vedere che è stata creata la directory

aa=0
bb="0"
intest = []
for y in x:                                                                                                 #ciclo for per controllare tutti i file
    for f in linnn:                                                                                         #ciclo for per controllare il contenuto del file di config
        linn = ''.join(f).split(":")                                                                        #divide la variabile "f" ogni due punti
        if('.' in y):                                                                                       #controlla se l'elemento selezionato dal ciclo for è un file
         b = ""                                                                                             #variabile con la lista dei file spostati
         z = y.split(".")                                                                                   #divide il file in pezzi ad ogni punto (".")
         a = len(z)-1                                                                                       #prende l'ultimo pezzo del file, dopo il punto così da prendere l'estensione
         for e in linn:                                                                                     #ciclo for per controllare l'estensione rispetto a quelle nel file
            aa=aa+1                                                                                         #aumenta di 1 il counter per il loop
            if(aa > 3):                                                                                     #crea la variabile col percorso in cui cercare se il file selezionato è già presente, ignorando le varie righe di configurazione nel file di config
                if("#" not in direc+lin[1]+"\\"+linn[len(linn)-1]):                                         #controlla se la linea corrente non è quella delle istruzioni nel file di config
                    if(lin[1] != linn[len(linn)-1]):                                                        #controlla se la linea corrente non è quella che definisce il nome della directory principale
                        intest = os.listdir(direc+lin[1]+"\\"+linn[len(linn)-1])                            #variabile in cui cercare il file selezionato
            if(y not in intest):                                                                            #controlla se il file selezionato non è già presente nella cartella di destinazione
                if(y != "File organizer.exe"):
                    if(e != linn[len(linn)-1]):                                                             #esegue il codice sotto solo se l'estemsione nel file presa attualmente non è il nome della directory dichiarata nel file
                        if(z[a] == e):                                                                      #controlla se l'estensione nel file è uguale a quella del file preso
                            d = b                                                                           #copia il contenuto della lista dei file spostati sulla variabile "d"
                            if(b != ""):                                                                    #controlla se c'è qualcosa nella lista dei file spostati (quindi se è stato spostato qualcosa in precedenza)
                                b = "\n"+b+f"Sto spostando {y} in "+direc+lin[1]+"\\"+linn[len(linn)-1]     #se si, aggiunge un ritorno a capo
                            else:
                                b = b+f"Sto spostando {y} in "+direc+lin[1]+"\\"+linn[len(linn)-1]          #altrimenti non lo aggiunge
                            print(b, end='\r')                                                              #scrive sul terminale che si sta spostando il file
                            dadir = direc+y                                                                 #dichiara la directory di partenza (con il file)
                            shutil.move(dadir, direc+lin[1]+"\\"+linn[len(linn)-1]+"\\")                    #sposta effettivamente il file
                            C=C+1
                            b = d                                                                           #la lista viene ripristinata allo stato precedente
                            if(b != ""):                                                                    #stessa cosa di prima, se la lista è piena
                                b = "\n"+b+y+" spostato in "+direc+lin[1]+"\\"+linn[len(linn)-1]+"                                                                                        "#aggiunge un ritorno a capo,
                            else:
                                b = b+y+" spostato in "+direc+lin[1]+"\\"+linn[len(linn)-1]+"                                                                                        "#altrimenti no
                            c=c+1                                                                           #aggiunge ogni volta 1 alla variabile c in modo da poter capire se è stato spostato qualcosa
                            print(b)                                                                        #scrive la lista dei file spostati
                    else:
                        b = b+y+"\n"                                                                        #se il file selezionato non corrisponde a nessun'estensione nel file di config, scrive nella variabile solo il nome del file
            elif(y in intest):                                                                              #controlla se il file selezionato è già presente in qualche cartella
                if(y not in bb):                                                                            #controlla se il nome del file è stato già messo nella variabile
                    if(bb == ""):                                                                           #se la variabile è vuota la crea senza virgole
                        bb=bb+y
                    else:
                        bb=bb+","+y                                                                         #altirmenti la crea con una virgola in modo da poter separare i nomi in seguito

if(c == 0):                                                                                                 #se ciò che è presente sopra non è stato fatto, scrive che non è stato spostato niente
    ff = bb.split(",")                                                                                      #variabile con i nomi dei file ignorati separati
    if(len(ff)-1 != 0):
        if(len(ff)-1 == 1):
            print(f"\n>>> Non è stato spostato nulla ({len(ff)-1} file ignorato, già presente nelle cartelle, {gg} directory create) <<<")
        elif(len(ff)-1 > 1):
            print(f"\n>>> Non è stato spostato nulla ({len(ff)-1} file ignorati, già presenti nelle cartelle, {gg} directory create) <<<")
    if(len(ff)-1 == 0):
        print(f"\n>>> Non è stato spostato nulla ({len(ff)-1} file ignorati, {gg} directory create) <<<")
else:
    print(hh)
    ff = bb.split(",")
    if(len(ff)-1-C != 0):
        if(len(ff)-1-C == 1):
            print(f"\n>>> Sono stati spostati {C} file ({len(ff)-1-C} file ignorato, già presente nelle cartelle, {gg} directory create) <<<")
        elif(len(ff)-1-C > 1):
            print(f"\n>>> Sono stati spostati {C} file ({len(ff)-1-C} file ignorati, già presenti nelle cartelle, {gg} directory create) <<<")
    if(len(ff)-1-C == 0):
        print(f"\n>>> Sono stati spostati {C} file ({len(ff)-1-C} file ignorati, {gg} directory create) <<<")
