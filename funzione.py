def copy_move(directory, configfile, datafile, optype, backup, delbackup, backfolder, assoluta, isbfolder):
    import shutil
    import os
    from os.path import dirname, abspath
    import magic
    import requests
    import json
    import win32com.client
    import inflect
    import stat
    from datetime import datetime
    import writer
    import backupper
    if("np.txt" not in os.listdir(r"C:\ProgramData\File Organizer")):
        with open(r"C:\ProgramData\File Organizer\np.txt", "w") as f:
            f.write('{"name":"null", "pdir":"null", "bfolder":"null"}')
    firma = "################################################################\n###   ####   ###  ###   ####   ###   ####   ###  ###  #####  ###\n###   ###   ####  ###   ###   ####   ###   ####  ###  #####  ###\n###   ##   #####  ###   ##   #####   ##   #####  ###  #####  ###\n###       ######  ###       ######       ######  ###  #####  ###\n###   ##   #####  ###   ##   #####   ##   #####  ###  #####  ###\n###   ###   ####  ###   ###   ####   ###   ####  ###  #####  ###\n###   ###   ####  ###   ###   ####   ###   ####  ###         ###\n#################################################versione:2.0.0#"
    print(firma)
    direc = directory
    with open("C:\ProgramData\File Organizer\config.txt") as f:
        lines = f.readlines()
    with open("C:\ProgramData\File Organizer\\np.txt") as f:
        dates = f.readlines()
    configdict = json.loads(lines[0])
    datadict = json.loads(dates[0])
    #nome_pdir = lines[1].split(":")[1]
    conf_dir = configdict["dir"]
    main_dir = os.listdir(direc)
    lista_file = os.listdir(direc)
    type_list = ""
    groups = {}
    main_dir_check=0
    num_of_created_dirs=0
    output = ""
    towrite = ""

    for file in main_dir:
        if(file == conf_dir):
            main_dir_check=main_dir_check+1
    #print(main_dir_check)
    create_main_dir_check=0
    if(main_dir_check == 0):
        num_of_created_dirs=num_of_created_dirs+1
        if(datadict["name"] == "null"):
            os.mkdir(direc+conf_dir)
            if(str(conf_dir).lower() in direc):
                os.rename(str(conf_dir).lower(), conf_dir)
            datadict["name"] = conf_dir
            datadict["pdir"] = assoluta
            out = f">>> Benvenuto <<<\n>>> Cartella creata: {direc}{conf_dir}\n"
            output = str(output) + str(out)
            print("main_dir_check == 0", datadict["name"])
            writer.wdata(json.dumps(datadict))
            create_main_dir_check=create_main_dir_check+1
        else:
            is_main_dir_present=0
            for file in main_dir:
                if("." not in file):
                    if((file == datadict["name"] and file != conf_dir) or (file.lower() == str(datadict["name"]).lower() and file.lower() != str(conf_dir).lower())):
                        is_main_dir_present=is_main_dir_present+1
            if(is_main_dir_present > 0):
                os.rename(direc+datadict["name"], direc+conf_dir)
                out = f">>> Cartella rinominata da",datadict["name"],"a",conf_dir+"\n"
                output = str(output) + str(out)
                datadict["name"] = conf_dir
                datadict["pdir"] = assoluta
                print("main dir check != 0, is main dir present 1")
                writer.wdata(json.dumps(datadict))
                create_main_dir_check=create_main_dir_check+1
            else:
                os.mkdir(direc+conf_dir)
                if(str(conf_dir).lower() in direc):
                    os.rename(str(conf_dir).lower(), conf_dir)
                out = f">>> La cartella principale non è presente, ne creo una: {direc+conf_dir}"+"\n"
                output = str(output) + str(out)
                datadict["name"] = conf_dir
                datadict["pdir"] = assoluta
                print("main dir check != 0, is main dir present 0")
                writer.wdata(json.dumps(datadict))
                create_main_dir_check=create_main_dir_check+1

    for file in lista_file:
        if(file != "desktop.ini" and file != "File Organizer.bat" and ".ini" not in file and ".sys" not in file and "Organizer.py" not in file and "Organizer" not in file and "organizer" not in file):
            if("." in file and os.path.isfile(direc+file) == True):
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
                if ("a" == "a"):
                    print("a == a")
                    folder = direc
                    filename = file
                    metadata = ['Name', 'Size', 'Item type', 'Date modified', 'Date created']
                    proprietà = get_file_metadata(folder, filename, metadata)
                if(proprietà["Item type"] not in groups):
                    print(proprietà["Item type"],"not in GROUPS",groups)
                    groups[proprietà["Item type"]]=[file]
                else:
                    groups[proprietà["Item type"]].append(file)
                if(proprietà["Item type"] not in type_list.split("\n")):
                    if(type_list == ""):
                        type_list=proprietà["Item type"]
                    else:
                        type_list=type_list+"\n"+proprietà["Item type"]

    file_check = ""
    enname = ""
    for x in range(len(groups)):
        nl = type_list.split("\n")
        print(f"groups = {groups}\nx = {x}\nnl = {nl}\nlen(nl) = {len(nl)}\nrange(len(groups)) = {range(len(groups))}\nnl[x] = {nl[x]}")
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

    target_dir = direc+conf_dir+"\\"
    target_dir_list = os.listdir(direc+conf_dir)
    tojoin = " "
    new = ""

    #check dei gruppi
    for grp_check in list(groups):
        if(grp_check.endswith(" ")):
            li = grp_check.rsplit(" ", 1)
            groups[new.join(li)] = groups.pop(grp_check)

    for dir_toCreate in groups:
        if(dir_toCreate not in target_dir_list):
            out = f">>> Cartella creata: {target_dir}{dir_toCreate}<<<\n"
            output = str(output) + str(out)
            os.mkdir(target_dir+dir_toCreate)
            num_of_created_dirs=num_of_created_dirs+1

    ignored_files=0
    moved_files = ""
    group_num=0
    mv_files_num=0

    if(backup == True and delbackup == False):
        backupper.backup(datetime, backfolder, new_type_list, new, groups, direc, stat, assoluta, isbfolder)

    elif(backup == True and delbackup == True):
        backupper.delbackup(datetime, backfolder, new_type_list, new, groups, direc, stat, assoluta, isbfolder)

    if(optype == "move"):
        for group_type in new_type_list.split("\n"):
            if(group_type.endswith(" ")):
                li = group_type.rsplit(" ", 1)
                group_type = new.join(li)
            if(group_type != ""):
                for group_num in range(len(groups[group_type])):
                    selected_file = groups[group_type][group_num]
                    check_final_dir = os.listdir(target_dir+group_type)
                    print(selected_file, str(check_final_dir).lower())
                    if(str(selected_file).lower() not in str(check_final_dir).lower()):
                        if("Organizer.py" not in selected_file):
                            moved_files = ""
                            shutil.move(direc+selected_file, target_dir+group_type)
                            out = f"{selected_file} spostato in {group_type}\n"
                            output = str(output) + str(out)
                            mv_files_num=mv_files_num+1
                    else:
                        ignored_files=ignored_files+1
    elif(optype == "copy"):
        for group_type in new_type_list.split("\n"):
            if(group_type.endswith(" ")):
                li = group_type.rsplit(" ", 1)
                group_type = new.join(li)
            if(group_type != ""):
                for group_num in range(len(groups[group_type])):
                    selected_file = groups[group_type][group_num]
                    check_final_dir = os.listdir(target_dir+group_type)
                    if(str(selected_file).lower() not in str(check_final_dir).lower()):
                        if("Organizer.py" not in selected_file):
                            moved_files = ""
                            shutil.copy(direc+selected_file, target_dir+group_type)
                            out = f"{selected_file} copiato in {group_type}\n"
                            output = str(output) + str(out)
                            mv_files_num=mv_files_num+1
                    else:
                        ignored_files=ignored_files+1

    text_file = open(r"C:\ProgramData\File Organizer\history.txt", "a")
    rawdata = datetime.now()
    datat = rawdata.strftime("%d/%m/%Y %H:%M:%S")
    finaldata = datat.replace("/", "-").replace(":", ".")
    tohistory = f"{output}\n\n/\-/\-/\-/\-/\-/\-/\-/\-/\-/\-/\-/\-/\-/\-/\-/\-/\-/\-/\-/\-/\-/\-/\-/\-/\-/\-/\-/\-/\-/\-/\-/\-/\ \n\nOPERAZIONE EFFETTUATA ALLE {finaldata}\n\n------------------------------------------------------------------------------------\n\n"
    n = text_file.write(tohistory)
    text_file.close()

    if(optype == "move"):
        if(mv_files_num == 0):
            if(ignored_files > 1 or ignored_files == 0):
                out = f">>> Non è stato spostato nulla ({ignored_files} file ignorati, {num_of_created_dirs} cartelle create) <<<"
                output = str(output) + str(out)
            else:
                out = f">>> Non è stato spostato nulla ({ignored_files} file ignorato, {num_of_created_dirs} cartelle create) <<<"
                output = str(output) + str(out)
        if((ignored_files > 1 or ignored_files == 0) and mv_files_num > 1):
            out = f">>> Sono stati spostati {mv_files_num} file ({ignored_files} file ignorati, {num_of_created_dirs} cartelle create) <<<"
            output = str(output) + str(out)
        elif(ignored_files == 1 and mv_files_num > 1):
            out = f">>> Sono stati spostati {mv_files_num} file ({ignored_files} file ignorato, {num_of_created_dirs} cartelle create) <<<"
            output = out
        elif(mv_files_num == 1 and (ignored_files > 1 or ignored_files == 0)):
            out = f">>> È stato spostato {mv_files_num} file ({ignored_files} file ignorati, {num_of_created_dirs} cartelle create) <<<"
            output = str(output) + str(out)
        return output
    elif(optype == "copy"):
        if(mv_files_num == 0):
            if(ignored_files > 1 or ignored_files == 0):
                out = f">>> Non è stato copiato nulla ({ignored_files} file ignorati, {num_of_created_dirs} cartelle create) <<<"
                output = str(output) + str(out)
            else:
                out = f">>> Non è stato copiato nulla ({ignored_files} file ignorato, {num_of_created_dirs} cartelle create) <<<"
                output = str(output) + str(out)
        if((ignored_files > 1 or ignored_files == 0) and mv_files_num > 1):
            out = f">>> Sono stati copiati {mv_files_num} file ({ignored_files} file ignorati, {num_of_created_dirs} cartelle create) <<<"
            output = str(output) + str(out)
        elif(ignored_files == 1 and mv_files_num > 1):
            out = f">>> Sono stati copiati {mv_files_num} file ({ignored_files} file ignorato, {num_of_created_dirs} cartelle create) <<<"
            output = str(output) + str(out)
        elif(mv_files_num == 1 and (ignored_files > 1 or ignored_files == 0)):
            out = f">>> È stato copiato {mv_files_num} file ({ignored_files} file ignorati, {num_of_created_dirs} cartelle create) <<<"
            output = str(output) + str(out)
        return output

def update(directory, configfile, datafile, new):
    import json
    import os
    output = ""
    is_main_dir_present = 0
    direc = directory
    if("np.txt" in os.listdir(r"C:\ProgramData\File Organizer")):
        with open("C:\ProgramData\File Organizer\config.txt") as f:
            lines = f.readlines()
        with open("C:\ProgramData\File Organizer\\np.txt") as f:
            data = f.readlines()
        datadict = json.loads(data[0])
        configdict = json.loads(lines[0])
        conf_dir = configdict["dir"]
        main_dir = os.listdir(direc)
        main_dir_check = 0
        for file in main_dir:
            if(file == new):
                main_dir_check=main_dir_check+1
            if(main_dir_check == 0):
                if((file == datadict["name"] and file != new) or (file.lower() == str(datadict["name"]).lower() and file.lower() != str(new).lower())):
                    is_main_dir_present=is_main_dir_present+1

        if(is_main_dir_present > 0):
            os.rename(direc+datadict["name"], direc+new)
            output = "Cartella rinominata"
            datadict["name"] = new
            text_file = open(r"C:\ProgramData\File Organizer\np.txt", "w")
            n = text_file.write(json.dumps(datadict))
            text_file.close()
        elif(new in main_dir):
            output = "La cartella ha già questo nome"
        else:
            output = "La cartella non è stata ancora creata"
    return output