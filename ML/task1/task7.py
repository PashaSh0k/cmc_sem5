def find_shortest(l):
    k = ""
    for i in l:
        if i.isalpha():
            k += i
        else:
            k += ' '
    k = k.split()
    return len(min(k, key=len)) if k else 0


