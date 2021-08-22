import shutil
import os
from os.path import dirname, abspath
import magic
import requests
import json
import win32com.client
import inflect
firma = "################################################################\n###   ####   ###  ###   ####   ###   ####   ###  ###  #####  ###\n###   ###   ####  ###   ###   ####   ###   ####  ###  #####  ###\n###   ##   #####  ###   ##   #####   ##   #####  ###  #####  ###\n###       ######  ###       ######       ######  ###  #####  ###\n###   ##   #####  ###   ##   #####   ##   #####  ###  #####  ###\n###   ###   ####  ###   ###   ####   ###   ####  ###  #####  ###\n###   ###   ####  ###   ###   ####   ###   ####  ###         ###\n#################################################versione:2.0.0#"
print(firma)
direc = os.path.dirname(os.getcwd())+"\\"
assoluta = dirname(abspath(__file__))
lines = ""
with open(assoluta+"\\config.txt") as f:
    lines = f.readlines()
with open(assoluta+"\\data.txt") as f:
    dati_test = f.readlines()
config = ''.join(lines).split("\n")
data_raw = ''.join(dati_test).split("\n")
conf_dir = config[1].split(":")
main_dir = os.listdir(direc)
lista_file = os.listdir(direc)
type_list = ""
groups = {}
main_dir_check=0
num_of_created_dirs=0

for file in main_dir:
    if(conf_dir[0] == "dir"):
        if(file == conf_dir[1]):
            main_dir_check=main_dir_check+1
#print(main_dir_check)

create_main_dir_check=0
if(main_dir_check == 0):
    num_of_created_dirs=num_of_created_dirs+1
    with open(assoluta+"\\data.txt") as fil:
        dati = fil.readlines()
    A = ''.join(dati).split("\n")
    pdir = ''.join(A[1]).split(",")
    if(pdir[1] != assoluta):
        directo = open("data.txt", "w")
        n = directo.write("name,null\n"+"pdir,"+assoluta)
        directo.close()
    with open(assoluta+"\\data.txt") as fil:
        dat = fil.readlines()
    B = ''.join(dat).split("\n")
    testdata = ''.join(B[0]).split(",")
    if(testdata[1] == "null"):
        os.mkdir(direc+conf_dir[1])
        print(">>> Benvenuto <<<\n>>> Cartella creata:",direc+conf_dir[1])
        text_file = open("data.txt", "w")
        n = text_file.write("name,"+conf_dir[1]+"\n"+"pdir,"+assoluta)
        text_file.close()
        create_main_dir_check=create_main_dir_check+1
    else:
        is_main_dir_present=0
        data = data_raw[0].split(",")
        for file in main_dir:
            if("." not in file):
                if(file == data[1]):
                    is_main_dir_present=is_main_dir_present+1
        if(is_main_dir_present > 0):
            os.rename(direc+testdata[1], direc+conf_dir[1])
            print(">>> Cartella rinominata da",testdata[1],"a",conf_dir[1])
            text_file = open("data.txt", "w")
            n = text_file.write("name,"+conf_dir[1]+"\n"+"pdir,"+assoluta)
            text_file.close()
            create_main_dir_check=create_main_dir_check+1
        else:
            print(f">>> La cartella principale non è presente, ne creo una: {direc+conf_dir[1]}")
            os.mkdir(direc+conf_dir[1])
            text_file = open("data.txt", "w")
            n = text_file.write("name,"+conf_dir[1]+"\n"+"pdir,"+assoluta)
            text_file.close()
            create_main_dir_check=create_main_dir_check+1

for file in lista_file:
    if(file != "desktop.ini" and file != "File Organizer.bat"):
        if("." in file):
            metadata = ['Name', 'Size', 'Item type', 'Date modified', 'Date created']
            def get_file_metadata(path, filename, metadata):
                sh = win32com.client.gencache.EnsureDispatch('Shell.Application', 0)
                ns = sh.NameSpace(path)
                file_metadata = dict()
                item = ns.ParseName(str(filename))
                for ind, attribute in enumerate(metadata):
                    attr_value = ns.GetDetailsOf(item, ind)
                    if attr_value:
                        file_metadata[attribute] = attr_value
                return file_metadata
            if __name__ == '__main__':
                folder = direc
                filename = file
                metadata = ['Name', 'Size', 'Item type', 'Date modified', 'Date created']
                proprietà = get_file_metadata(folder, filename, metadata)
            if(proprietà["Item type"] not in groups):
                groups[proprietà["Item type"]]=[file]
            else:
                groups[proprietà["Item type"]].append(file)
            if(proprietà["Item type"] not in type_list):
                if(type_list == ""):
                    type_list=proprietà["Item type"]
                else:
                    type_list=type_list+"\n"+proprietà["Item type"]

file_check = ""
enname = ""
for x in range(len(groups)):
    nl = type_list.split("\n")
    if(len(groups[nl[x]]) != 0):
        selection = groups[nl[x]][len(groups[nl[x]])-1]
        #print(nl[x].lower()+" == file "+selection.split(".")[len(selection.split("."))-1])
        if(nl[x].lower() == "file "+selection.split(".")[len(selection.split("."))-1]):
            command = selection.split(".")
            rawcom = os.popen("assoc ."+command[len(command)-1]).read().split("=")
            if(len(rawcom)-1 != 0):
                if(file_check == ""):
                    file_check=str(x)
                else:
                    file_check=file_check+"\n"+str(x)
                comando = rawcom[len(rawcom)-1].replace(".", " ").replace("\n", "")
                if("file" in comando):
                    charcheck = 0
                    for lettera in comando:
                        if(lettera in command[len(command)-1]):
                            charcheck=charcheck+1
                    if(charcheck >= len(command[1])-1):
                        mime = magic.Magic(mime=True)
                        tipo_file_raw = mime.from_file(direc+selection)
                        tipo_file = tipo_file_raw.split("/")
                        if(enname == ""):
                            enname = tipo_file[0].capitalize()
                        else:
                            enname = enname+","+tipo_file[0].capitalize()
                else:
                    if(enname == ""):
                            enname = comando
                    else:
                        enname = enname+","+comando

itaresp = {}
if(enname != ""):
    p = inflect.engine()
    plurali = ""
    for parola in enname.split(","):
        if(plurali == ""):
            plurali = p.plural(parola)
        else:
            plurali = plurali+","+p.plural(parola)
    #print(plurali)
    list_type = ""
    for tipo in type_list.split("\n"):
        if(list_type == ""):
            list_type = tipo
        else:
            list_type = list_type+" ,"+tipo
    #print(list_type)
    url = "https://microsoft-translator-text.p.rapidapi.com/translate"
    querystring = {"api-version":"3.0","to":"it","textType":"plain","profanityAction":"NoAction"}
    payload = "[\r\n    {\r\n        \"Text\": \""+plurali+" ,"+list_type+"\"\r\n    }\r\n]"
    headers = {
        'content-type': "application/json",
        'x-rapidapi-host': "microsoft-translator-text.p.rapidapi.com",
        'x-rapidapi-key': "34221de93cmshab1788fe470ce0bp12eebejsndd89b2f16de8"
        }
    response = requests.request("POST", url, data=payload, headers=headers, params=querystring)
    resp_dict = json.loads(response.text)
    itaresp = resp_dict[0]["translations"][0]["text"]
    #print(itaresp)

new_type_list = type_list
new_split = str(str(itaresp).split(","))
da_sostituire = ""
for x in range(len(file_check.split("\n"))):
    resp_split = str(itaresp).split(",")
    #print(resp_split)
    nl = file_check.split("\n")
    if(file_check != ''):
        test = type_list.split("\n")[int(nl[x])]
        if(resp_split[x] in groups):
            groups[resp_split[x]] = groups[resp_split[x]]+groups[test]
            del groups[test]
            new_type_list = new_type_list.replace(test, "")
        else:
            groups[resp_split[x]] = groups.pop(test)
            new_type_list = new_type_list.replace(test, resp_split[x])
            new_split = new_split.replace(test, resp_split[x])
            if(da_sostituire == ""):
                da_sostituire=resp_split[x]+", "
            else:
                da_sostituire=da_sostituire+resp_split[x]+", "

target_dir = direc+conf_dir[1]+"\\"
target_dir_list = os.listdir(direc+conf_dir[1])

for dir_toCreate in groups:
    if(dir_toCreate not in target_dir_list):
        print(">>> Cartella creata:",target_dir+dir_toCreate,"<<<")
        os.mkdir(target_dir+dir_toCreate)
        num_of_created_dirs=num_of_created_dirs+1

ignored_files=0
moved_files = ""
group_num=0
mv_files_num=0
for group_type in new_type_list.split("\n"):
    if(group_type != ""):
        for group_num in range(len(groups[group_type])):
            selected_file = groups[group_type][group_num]
            check_final_dir = os.listdir(target_dir+group_type)
            if(selected_file not in check_final_dir):
                moved_files = ""
                print(f"Sto spostando {selected_file} in {target_dir+group_type}", end = '\r')
                shutil.move(direc+selected_file, target_dir+group_type)
                print(f"{selected_file} spostato in {target_dir+group_type}"+"                                                                                          "+"\n")
                mv_files_num=mv_files_num+1
            else:
                ignored_files=ignored_files+1

if(mv_files_num == 0):
    if(ignored_files > 1 or ignored_files == 0):
        print(f">>> Non è stato spostato nulla ({ignored_files} file ignorati, {num_of_created_dirs} cartelle create) <<<")
    else:
        print(f">>> Non è stato spostato nulla ({ignored_files} file ignorato, {num_of_created_dirs} cartelle create) <<<")
if((ignored_files > 1 or ignored_files == 0) and mv_files_num > 1):
    print(f">>> Sono stati spostati {mv_files_num} file ({ignored_files} file ignorati, {num_of_created_dirs} cartelle create) <<<")
elif(ignored_files == 1 and mv_files_num > 1):
    print(f">>> Sono stati spostati {mv_files_num} file ({ignored_files} file ignorato, {num_of_created_dirs} cartelle create) <<<")
elif(mv_files_num == 1 and (ignored_files > 1 or ignored_files == 0)):
    print(f">>> È stato spostato {mv_files_num} file ({ignored_files} file ignorati, {num_of_created_dirs} cartelle create) <<<")
