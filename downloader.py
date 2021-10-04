#autoupdate
def update(versione, direc):
    import requests
    import urllib.request
    import zipfile
    import os
    import shutil
    aggiornamento = 0
    get_hum_ver = ""
    try:
        link = "https://github.com/Kikkiu17/File-organizer-ITA/tags"
        f = requests.get(link)
        get_ver = f.text.split("/Kikkiu17/File-organizer-ITA/releases/tag/v")[1].split("\"")[0].replace(".", "")
        get_hum_ver = f.text.split("/Kikkiu17/File-organizer-ITA/releases/tag/v")[1].split("\"")[0]
        if(versione < int(get_ver)):
            print("NEW VERSION AVAILABLE")
            url = "https://github.com/Kikkiu17/File-organizer-ITA/archive/refs/heads/main.zip"
            file_name, headers = urllib.request.urlretrieve(url)
            with zipfile.ZipFile(file_name, 'r') as zip_ref:
                if("Update" not in os.listdir(r"C:\ProgramData\File Organizer")):
                    os.mkdir(r"C:\ProgramData\File Organizer\Update")
                if(len(str(os.listdir(r"C:\ProgramData\File Organizer\Update"))) != 0):
                    shutil.rmtree(r"C:\ProgramData\File Organizer\Update")
                    os.mkdir(r"C:\ProgramData\File Organizer\Update")
                zip_ref.extractall(r"C:\ProgramData\File Organizer\Update")
                dir_in_update = os.listdir(r"C:\ProgramData\File Organizer\Update")[0]
                os.rename(fr"C:\ProgramData\File Organizer\Update\{dir_in_update}\Organizer.py", fr"C:\ProgramData\File Organizer\Update\{dir_in_update}\Organizer-v{get_hum_ver}.py")
                shutil.move(fr"C:\ProgramData\File Organizer\Update\{dir_in_update}\Organizer-v{get_hum_ver}.py", direc)
            aggiornamento = 1
    except:
        print("Impossibile contattare il server")
    return aggiornamento, get_hum_ver