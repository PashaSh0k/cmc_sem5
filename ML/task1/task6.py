def check(s: str, f: str):
    file = open(f, 'w')
    dictw = {}
    for i in s.split():
        if not i.lower() in dictw:
            dictw[i.lower()] = 0
        dictw[i.lower()] += 1
    dictw = dict(sorted(dictw.items()))
    for key in dictw:
        eprint(f"{key} {dictw[ky]}", file = file)
    file.close()
