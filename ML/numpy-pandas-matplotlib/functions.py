from typing import List


def sum_non_neg_diag(X: List[List[int]]) -> int:
    """
    Вернуть  сумму неотрицательных элементов на диагонали прямоугольной матрицы X. 
    Если неотрицательных элементов на диагонали нет, то вернуть -1
    """

    s = 0
    flag = 0
    minim = min(len(X), len(X[0]))
    for i in range(minim):
        if X[i][i] >= 0:
            s += X[i][i]
            flag = 1
    return s if flag else -1


def are_multisets_equal(x: List[int], y: List[int]) -> bool:
    """
    Проверить, задают ли два вектора одно и то же мультимножество.
    """

    if len(x) != len(y):
        return False
    x = sorted(x)
    y = sorted(y)
    return x == y


def max_prod_mod_3(x: List[int]) -> int:
    """
    Вернуть максимальное прозведение соседних элементов в массиве x, 
    таких что хотя бы один множитель в произведении делится на 3.
    Если таких произведений нет, то вернуть -1.
    """

    if len(x) < 2:
        return -1
    maxs = -1
    for i in range(1, len(x) - 1):
        if x[i] % 3 == 0:
            m = max(x[i] * x[i-1], x[i] * x[i+1])
            if m > maxs:
                maxs = m
    m = x[0] * x[1]
    if m % 3 == 0 and m > maxs:
        maxs = m
    m = x[len(x) - 1] * x[len(x)-2]
    if m % 3 == 0 and m > maxs:
        maxs = m
    return maxs



def convert_image(image: List[List[List[float]]], weights: List[float]) -> List[List[float]]:
    """
    Сложить каналы изображения с указанными весами.
    """
    result = []
    i = 0
    for matrix in image:
        result.append([])
        for array in matrix:
            sum = 0
            for count in range(len(weights)):
                sum += array[count] * weights[count]
            result[i].append(sum)
        i += 1
    return result


def rle_scalar(x: List[List[int]], y:  List[List[int]]) -> int:
    """
    Найти скалярное произведение между векторами x и y, заданными в формате RLE.
    В случае несовпадения длин векторов вернуть -1.
    """

    first = []
    second = []
    for i in range(len(x)):
        for k in range((x[i][1])):
            first.append(x[i][0])
    for i in range(len(y)):
        for k in range((y[i][1])):
            second.append(y[i][0])
    if len(first) != len(second):
        return -1
    scal = 0
    for i in range(len(first)):
        scal += first[i] * second[i]
    return int(scal)


def cosine_distance(X: List[List[float]], Y: List[List[float]]) -> List[List[float]]:
    """
    Вычислить матрицу косинусных расстояний между объектами X и Y. 
    В случае равенства хотя бы одно из двух векторов 0, косинусное расстояние считать равным 1.
    """

    matrix = []
    list_of_zeros = [0] * len(X[0])
    list_of_units = [1] * len(X[0])
    for i in range(len(X)):
        if X[i] == list_of_zeros:
            matrix.append(list_of_units)
            continue
        matrix.append([])
        sum1 = 0
        for k in range(len(X[i])):
            sum1 += X[i][k] * X[i][k]
        length1 = sum1 ** (1/2)
        for j in range(len(Y)):
            if Y[j] == list_of_zeros:
                matrix[i].append(1)
                continue
            sum2 = 0
            for k in range(len(Y[j])):
                sum2 += Y[j][k] * Y[j][k]
            length2 = sum2 ** (1 / 2)
            scal = 0
            for g in range(len(X[i])):
                scal += X[i][g] * Y[j][g]
            cos = scal / (length1 * length2)
            matrix[i].append(cos)
    return matrix




