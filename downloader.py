#autoupdate
def update(versione, direc):
    import requests
    import urllib.request
    import zipfile
    import os
    import shutil
    aggiornamento = 0
    get_hum_ver = ""
    link = "https://github.com/Kikkiu17/File-organizer-ITA/tags"
    f = requests.get(link)
    get_ver = f.text.split("/Kikkiu17/File-organizer-ITA/releases/tag/v")[1].split("\"")[0].replace(".", "")
    get_hum_ver = f.text.split("/Kikkiu17/File-organizer-ITA/releases/tag/v")[1].split("\"")[0]
    print(f"if {versione} < {int(get_ver)}")
    if(versione < int(get_ver)):
        print("new ver:",get_hum_ver)
        url = f"https://github.com/Kikkiu17/File-organizer-ITA/releases/download/v{get_hum_ver}/Organizer-v{get_hum_ver}.exe"
        import urllib.request
        import shutil
        if("Update" not in os.listdir(r"C:\ProgramData\File Organizer")):
            os.mkdir(r"C:\ProgramData\File Organizer\Update")
            text_file = open(rf"C:\ProgramData\File Organizer\Update\Organizer-v{get_hum_ver}.txt", "a")
            n = text_file.write("")
            text_file.close()
            os.rename(rf"C:\ProgramData\File Organizer\Update\Organizer-v{get_hum_ver}.txt", rf"C:\ProgramData\File Organizer\Update\Organizer-v{get_hum_ver}.exe")
        if(len(str(os.listdir(r"C:\ProgramData\File Organizer\Update"))) != 0):
            shutil.rmtree(r"C:\ProgramData\File Organizer\Update")
            os.mkdir(r"C:\ProgramData\File Organizer\Update")
            text_file = open(rf"C:\ProgramData\File Organizer\Update\Organizer-v{get_hum_ver}.txt", "a")
            n = text_file.write("")
            text_file.close()
            os.rename(rf"C:\ProgramData\File Organizer\Update\Organizer-v{get_hum_ver}.txt", rf"C:\ProgramData\File Organizer\Update\Organizer-v{get_hum_ver}.exe")
        with urllib.request.urlopen(url) as response, open(rf"C:\ProgramData\File Organizer\Update\Organizer-v{get_hum_ver}.exe", 'wb') as out_file:
            shutil.copyfileobj(response, out_file)
        aggiornamento = 1
    return aggiornamento, get_hum_ver