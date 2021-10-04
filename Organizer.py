import os
from posixpath import split
import PySimpleGUI as sg
import os.path
import json
from datetime import datetime
from os.path import dirname, abspath
import funzione
import downloader
import time
import sys
configfile = 'C:\ProgramData\File Organizer\config.txt'
datafile = 'C:\ProgramData\File Organizer\data.txt'
backup = False
delbackup = False
pdirset = ""
assoluta = ""
direc = os.path.dirname(os.getcwd())+"\\"
if getattr(sys, 'frozen', False):
    assoluta = os.path.dirname(sys.executable)
    running_mode = 'Frozen/executable'
else:
    try:
        app_full_path = os.path.realpath(__file__)
        assoluta = os.path.dirname(app_full_path)
        running_mode = "Non-interactive (e.g. 'python myapp.py')"
    except NameError:
        assoluta = os.getcwd()
        running_mode = 'Interactive'

print('Running mode:', running_mode)
print('  Appliction path  :', assoluta)
versione = 321
backfolder = ""
isbfolder = 0

if("INSTALLA_QUI.bat" in os.listdir(dirname(abspath(__file__)))):
    os.remove(dirname(abspath(__file__))+"\\INSTALLA_QUI.bat")
if("py-lib.ps1" in os.listdir(dirname(abspath(__file__)))):
    os.remove(dirname(abspath(__file__))+"\\py-lib.ps1")

#crea file necessari se non ci sono già
if("file organizer" in os.listdir(r"C:\ProgramData")):
    os.rename(r"C:\ProgramData\file Organizer", r"C:\ProgramData\File Organizer")
if("file organizer" in os.listdir(r"C:\ProgramData")):
    os.rename(r"C:\ProgramData\file Organizer", r"C:\ProgramData\File Organizer")
if("File Organizer" not in os.listdir(r"C:\ProgramData")):
    print("file organizer not in programdata")
    configdict = '{"dir" : "File sistemati"}'
    datadict = '{"name": "null", "pdir": "null", "backup": "off", "delbackup": "off"}'
    os.mkdir(r"C:\ProgramData\File Organizer")
    text_file = open(r"C:\ProgramData\File Organizer\data.txt", "w")
    n = text_file.write(datadict)
    text_file.close()
    text_file = open(r"C:\ProgramData\File Organizer\config.txt", "w")
    n = text_file.write(configdict)
    text_file.close()
if("config.txt" not in os.listdir(r"C:\ProgramData\File Organizer")):
    print("config.txt not in file organizer")
    configdict = '{"dir" : "File sistemati"}'
    text_file = open(r"C:\ProgramData\File Organizer\config.txt", "w")
    n = text_file.write(configdict)
    text_file.close()
if("data.txt" not in os.listdir(r"C:\ProgramData\File Organizer")):
    print("data.txt not in file organizer")
    datadict = '{"name": "null", "pdir": "null", "backup": "off", "delbackup": "off"}'
    text_file = open(r"C:\ProgramData\File Organizer\data.txt", "w")
    n = text_file.write(datadict)
    text_file.close()

isaggiornamento = downloader.update(versione, assoluta)

for file in os.listdir(assoluta):
    if("Organizer-v" in file):
        splitted = file.split("Organizer-v")[1].split(".")
        final = ""
        x = 0
        for lettera in splitted:
            x = x + 1
            if(x < len(splitted)):
                final = final+lettera
        if(int(final) < versione):
            os.remove(assoluta+"\\"+file)

#imposta la gui
sg.theme('SystemDefault1')
nums = os.listdir(direc)
def vsep():
    return sg.Text("")
sz=(10,20)
col1 = [
    [
        sg.Text("Scegli la cartella da riordinare"),
        sg.In(size=(119, 1), enable_events=True, key="-FOLDER-"),
        sg.FolderBrowse("Sfoglia..."),
    ],
    [
        sg.Listbox(
            values=[], enable_events=True, size=(75, 15), key="-FILE LIST-"
        ), sg.Listbox(
             values=[], enable_events=True, size=(75, 15), key="-output-"
        )
    ],
    [
        sg.Button(
            'Sposta tutto'
            ), sg.Button(
                "Copia tutto", key="copia"
                ), sg.Button(
                    "Aggiorna", key="upd"
                    ), sg.Text(
                        datetime.now().strftime("%H:%M:%S")+" - "+"Pronto", key="stato"
                        )
    ],
    [
        sg.Text(
            "Cartella di backup:",
            ), sg.In(
                size=(35,1), enable_events=True, key="bfolder"
                ), sg.FolderBrowse(
                    "Sfoglia..."
                    ),
    ],
    [
        sg.Checkbox(
            "Crea backup", key="cbackup", enable_events=True
            ), sg.Checkbox(
                "Elimina backup precedenti", key="ebackup", enable_events=True
            ), sg.Text(
                "Nome cartella principale:"
            ), sg.In(
                size=(20, 1), enable_events=True, key="parentdir"
                ), sg.Button(
                    "Imposta", key="dirset"
                )
    ]
]
layout = [
    [
        sg.Column(col1, element_justification='l')
    ]
]
window = sg.Window("File Organizer", layout, resizable=True, finalize=True)
window['-output-'].expand(expand_x=True, expand_y=True, expand_row=True)
tojoin = ""
lista_file = []

#legge i file e imposta le variabili nella gui
with open(configfile) as f:
    lines = f.readlines()
nome_pdir = json.loads(lines[0])
window["parentdir"].update(str(nome_pdir["dir"]))

with open(datafile) as f:
    lines = f.readlines()
data = json.loads(lines[0])
if(data["backup"] == "on"):
    window["cbackup"].update(value=True)
    window["copia"].update(disabled=True)
    backup = True
if(data["delbackup"] == "on"):
    window["ebackup"].update(value=True)
    delbackup = True

if("backupdata.txt" in os.listdir(r"C:\ProgramData\File Organizer")):
    with open(r"C:\ProgramData\File Organizer\backupdata.txt") as f:
        backdataraw = f.readlines()
    backdata = json.loads(backdataraw[0])
    if(backdata["bfolder"] != "null"):
        backfolder = backdata["bfolder"]
        window["stato"].update(f"La cartella di backup è {backfolder}")

if(isaggiornamento[0] == 1):
    window["stato"].update(f"C'è una nuova versione del programma! Chiudi questa finestra e apri il nuovo file, Organizer{isaggiornamento[1]}.py!")
    window["-FILE LIST-"].update(f"C'è una nuova versione del programma! Guarda lo stato")
    window["-output-"].update(f"C'è una nuova versione del programma! Guarda lo stato")

#controlla gli eventi della gui
while True:
    event, values = window.read()
    if event == "Exit" or event == sg.WIN_CLOSED:
        break
    elif event == "-FOLDER-":
        if(lista_file != []):
            lista_file = []
        cart = values["-FOLDER-"]
        cartella = cart.replace("/", "\\")+"\\"
        file_list = os.listdir(cartella)
        for file_sel in file_list:
            if(file_sel != "desktop.ini"):
                if(os.path.isfile(cartella+file_sel) == True):
                    lista_file.append(file_sel)
        window["-FILE LIST-"].update(lista_file)
    elif event == "Sposta tutto":
        if(lista_file == []):
            window["stato"].update(datetime.now().strftime("%H:%M:%S")+" - "+"Pronto - Devi selezionare una cartella")
        else:
            if(isbfolder == 0 and backup == False):
                window["stato"].update(f"La cartella per i backup è stata selezionata automaticamente su {assoluta}")
                time.sleep(2)
            window["stato"].update(datetime.now().strftime("%H:%M:%S")+" - "+"Spostamento in corso")
            optype = "move"
            sposta = funzione.copy_move(cartella, configfile, datafile, optype, backup, delbackup, backfolder, assoluta, isbfolder)
            window["parentdir"].update(str(nome_pdir["dir"]))
            window["-output-"].update(sposta.split("\n"))
            window["stato"].update(datetime.now().strftime("%H:%M:%S")+" - "+sposta.split("\n")[len(sposta.split("\n"))-1])
    elif event == "copia":
        if(lista_file == []):
            window["stato"].update(datetime.now().strftime("%H:%M:%S")+" - "+"Pronto - Devi selezionare una cartella")
        else:
            if(isbfolder == 0 and backup == False):
                window["stato"].update(f"La cartella per i backup è stata selezionata automaticamente su {assoluta}")
                time.sleep(2)
            window["stato"].update(datetime.now().strftime("%H:%M:%S")+" - "+"Copia in corso")
            optype = "copy"
            sposta = funzione.copy_move(cartella, configfile, datafile, optype, backup, delbackup, backfolder, assoluta, isbfolder)
            window["parentdir"].update(str(nome_pdir["dir"]))
            window["-output-"].update(sposta.split("\n"))
            window["stato"].update(datetime.now().strftime("%H:%M:%S")+" - "+sposta.split("\n")[len(sposta.split("\n"))-1])
    elif event == "cbackup":
        if values["cbackup"] == True:
            window["stato"].update(datetime.now().strftime("%H:%M:%S")+" - "+"Pronto - Backup abilitato")
            backup = True
            window["copia"].update(disabled=True)
            data["backup"] = "on"
        elif values["cbackup"] == False:
            window["stato"].update(datetime.now().strftime("%H:%M:%S")+" - "+"Pronto - Backup disabilitato")
            backup = False
            window["copia"].update(disabled=False)
            data["backup"] = "off"
    elif event == "ebackup":
        if values["ebackup"] == True:
            window["stato"].update(datetime.now().strftime("%H:%M:%S")+" - "+"Pronto - I backup precedenti verranno eliminati")
            delbackup = True
            data["delbackup"] = "on"
        elif values["ebackup"] == False:
            window["stato"].update(datetime.now().strftime("%H:%M:%S")+" - "+"Pronto - I backup precedenti non verranno eliminati")
            delbackup = False
            data["delbackup"] = "off"
    elif event == "upd":
        if(lista_file != []):
            lista_file = []
        cart = values["-FOLDER-"]
        cartella = cart.replace("/", "\\")+"\\"
        file_list = os.listdir(cartella)
        for file_sel in file_list:
            if(file_sel != "desktop.ini"):
                if(os.path.isfile(cartella+file_sel) == True):
                    lista_file.append(file_sel)
        window["-FILE LIST-"].update(lista_file)
    elif event == "dirset":
        cart = values["-FOLDER-"]
        cartella = cart.replace("/", "\\")+"\\"
        nome_pdir["dir"] = values["parentdir"]
        new = nome_pdir
        print("dirset event")
        text_file = open(configfile, "w")
        n = text_file.write(json.dumps(nome_pdir))
        text_file.close()
        update = funzione.update(cartella, configfile, datafile, nome_pdir["dir"])
        window["stato"].update(datetime.now().strftime("%H:%M:%S")+" - "+update)
    elif event == "bfolder":
        isbfolder = 1
        backfolder = values["bfolder"]
        if("backupdata.txt" in os.listdir(r"C:\ProgramData\File Organizer")):
            with open(r"C:\ProgramData\File Organizer\backupdata.txt") as f:
                npfile = f.readlines()
            npdict = json.loads(npfile[0])
            npdict["bfolder"] = backfolder
            with open(r"C:\ProgramData\File Organizer\backupdata.txt", "w") as f:
                f.write(json.dumps(npdict))
        else:
            with open(r"C:\ProgramData\File Organizer\backupdata.txt", "w") as f:
                f.write('{"bfolder":"null"}')
            with open(r"C:\ProgramData\File Organizer\backupdata.txt") as f:
                npfile = f.readlines()
            npdict = json.loads(npfile[0])
            npdict["bfolder"] = backfolder
            with open(r"C:\ProgramData\File Organizer\backupdata.txt", "w") as f:
                f.write(json.dumps(npdict))
        window["stato"].update(datetime.now().strftime("%H:%M:%S")+" - "+"Cartella di backup impostata")
    text_file = open(datafile, "w")
    n = text_file.write(json.dumps(data))
    text_file.close()
window.close()

