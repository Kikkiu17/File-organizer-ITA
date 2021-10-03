import os
import PySimpleGUI as sg
import os.path
import json
from datetime import datetime
import sys
from os.path import dirname, abspath
import shutil
import urllib.request
import zipfile
configfile = 'C:\ProgramData\File Organizer\config.txt'
datafile = 'C:\ProgramData\File Organizer\data.txt'
backup = False
delbackup = False
pdirset = ""

#autoupdate
#url = "https://github.com/Kikkiu17/File-organizer-ITA/archive/refs/heads/main.zip"
#file_name, headers = urllib.request.urlretrieve(url)
#with zipfile.ZipFile(file_name, 'r') as zip_ref:
#    zip_ref.extractall(r"C:\ProgramData\File Organizer")
#os.popen("python C:\ProgramData\File Organizer\update.py")

if("INSTALLA_QUI.bat" in os.listdir(dirname(abspath(__file__)))):
    os.remove(dirname(abspath(__file__))+"\\INSTALLA_QUI.bat")
if("py-lib.ps1" in os.listdir(dirname(abspath(__file__)))):
    os.remove(dirname(abspath(__file__))+"\\py-lib.ps1")

#crea file necessari se non ci sono già
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

if("funzione.py" in os.listdir(dirname(abspath(__file__))) and "funzione.py" not in os.listdir('C:\ProgramData\File Organizer')):
    shutil.move(dirname(abspath(__file__))+"\\funzione.py", 'C:\ProgramData\File Organizer')
if("writer.py" in os.listdir(dirname(abspath(__file__))) and "writer.py" not in os.listdir('C:\ProgramData\File Organizer')):
    shutil.move(dirname(abspath(__file__))+"\\writer.py", 'C:\ProgramData\File Organizer')

sys.path.insert(1, 'C:\ProgramData\File Organizer')
import funzione

#imposta la gui
sg.theme('SystemDefault1')
direc = os.path.dirname(os.getcwd())+"\\"
nums = os.listdir(direc)
file_list_column = [
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
                )
    ],
    [
        sg.Text(datetime.now().strftime("%H:%M:%S")+" - "+"Pronto", key="stato")
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
        sg.Column(file_list_column),
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
            window["stato"].update(datetime.now().strftime("%H:%M:%S")+" - "+"Spostamento in corso")
            optype = "move"
            sposta = funzione.copy_move(cartella, configfile, datafile, optype, backup, delbackup)
            window["parentdir"].update(str(nome_pdir["dir"]))
            window["-output-"].update(sposta.split("\n"))
            window["stato"].update(datetime.now().strftime("%H:%M:%S")+" - "+sposta.split("\n")[len(sposta.split("\n"))-1])
    elif event == "copia":
        if(lista_file == []):
            window["stato"].update(datetime.now().strftime("%H:%M:%S")+" - "+"Pronto - Devi selezionare una cartella")
        else:
            window["stato"].update(datetime.now().strftime("%H:%M:%S")+" - "+"Copia in corso")
            optype = "copy"
            sposta = funzione.copy_move(cartella, configfile, datafile, optype, backup, delbackup)
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
    text_file = open(datafile, "w")
    n = text_file.write(json.dumps(data))
    text_file.close()
window.close()

