from typing import List


def hello(name: str = None) -> str:
    if name:
        return f"Hello, {name}!"
    else:
        return "Hello!"


def int_to_roman(num: int) -> str:
    first = ["", "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX"]
    second = ["", "X", "XX", "XXX", "XL", "L", "LX", "LXX", "LXXX", "XC"]
    third = ["", "C", "CC", "CCC", "CD", "D", "DC", "DCC", "DCCC", "CM"]
    fourth = ["", "M", "MM", "MMM"]

    return fourth[num // 1000] + third[num // 100 % 10] + second[num // 10 % 10] + first[num % 10]

def longest_common_prefix(strs_input) -> str:
    result = ""
    if strs_input == []:
        return result
    sp = []
    sp.extend(strs_input)
    s = []
    for w in sp:
        s.append(w.strip())
    i = 0
    s.sort()
    print(s)
    for i in range(len(s[0])):
        if s[0][i] == s[len(s) - 1][i]:
            result += s[0][i]
        else:
            return result
    return result

def check(x : int) -> int:
    k = 0
    for i in range(2, x // 2 + 1):
        if(x % i == 0):
            k += 1
    if k:
        return 0
    else:
        return 1
def primes() -> int:
    i = 2
    while 1:
        if check(i):
            yield i
        i += 1



class BankCard:
    def __init__(self, total_sum: int, balance_limit: int = None):
        self.total_sum = total_sum
        self.balance_limit = balance_limit

    def __call__(self, sum_spent):
        if sum_spent > self.total_sum:
            raise ValueError("Not enough money to spend sum_spent dollars.")
        else:
            self.total_sum -= sum_spent
            print(f"You spent {sum_spent} dollars")
    @property
    def balance(self):
        if self.balance_limit == 0:
            raise ValueError("Balance check limits exceeded.")
        if self.balance_limit is not None:
            self.balance_limit -= 1
        return self.total_sum
    def __str__(self):
        return "To learn the balance call balance."
    def put(self, sum_put: int):
        print(f"You зut {sum_put} dollars")
        self.total_sum += sum_put
    def __add__(self, other):
        if(self.balance_limit == None) or (other.balance_limit == None):
            return BankCard(self.total_sum + other.total_sum, None)
        else:
            return BankCard(self.total_sum + other.total_sum, max(self.balance_limit, other.balance_limit))