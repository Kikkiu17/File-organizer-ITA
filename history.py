def cronologia():
    import PySimpleGUI as sg
    import os
    can_history = 0
    if("history.txt" in os.listdir(r"C:\ProgramData\File Organizer")):
        with open(r"C:\ProgramData\File Organizer\history.txt") as f:
            history = f.readlines()
            can_history = 1
            if(history != ""):
                can_history = 2
                print(can_history)
    layout = [
        [
            sg.Listbox(
              values=[], enable_events=True, size=(150, 25), key="lista_cron"
              )
        ],
        [
            sg.Button(
                "Cancella cronologia", key="delete_cron"
            ), sg.Button(
                "Chiudi"
            )
        ],
        [
            sg.Text(
                "Pronto", key="stato"#FIXARE File "history.py", line 44, in cronologia io.UnsupportedOperation: not writable FIXARE CARTELLE CREATE CON LETTERA MINUSCOLA
            )
        ]
    ]
    window = sg.Window("Cronologia", layout, modal=True, finalize=True)
    choice = None
    if(can_history == 0):
        window["stato"].update("Il file di cronologia non è presente")
    elif(can_history == 1):
        window["stato"].update("Il file di cronologia è vuoto")
    elif(can_history == 2):
        window["lista_cron"].update(history)
        window["stato"].update("Cronologia caricata da file history.txt")
    while True:
        event, values = window.read()
        if event == "Exit" or event == sg.WIN_CLOSED or event == "Chiudi":
            break
        if event == "delete_cron":
            if(can_history == 2):
                if("history.txt" in os.listdir(r"C:\ProgramData\File Organizer")):
                    with open(r"C:\ProgramData\File Organizer\history.txt") as f:
                        updhistory = str(f.readlines()).split("\n")
                    text_file = open(r"C:\ProgramData\File Organizer\history.txt", "w")
                    text_file.write(" ")
                    text_file.close()
                    window["stato"].update("Cronologia eliminata")
                    window["lista_cron"].update(updhistory)
        
    window.close()