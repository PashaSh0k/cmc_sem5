import numpy as np


def sum_non_neg_diag(X: np.ndarray) -> int:
    """
    Вернуть  сумму неотрицательных элементов на диагонали прямоугольной матрицы X. 
    Если неотрицательных элементов на диагонали нет, то вернуть -1
    """
    y = np.diag(X)
    if np.all(y < 0):
        return -1
    else:
        return sum(y[y >= 0])


def are_multisets_equal(x: np.ndarray, y: np.ndarray) -> bool:
    """
    Проверить, задают ли два вектора одно и то же мультимножество.
    """
    return (np.sort(x) == np.sort(y)).all()


def max_prod_mod_3(x: np.ndarray) -> int:
    """
    Вернуть максимальное прозведение соседних элементов в массиве x, 
    таких что хотя бы один множитель в произведении делится на 3.
    Если таких произведений нет, то вернуть -1.
    """
    if len(x) < 2:
        return -1
    y = np.roll(x, 1)
    y[0] = 0
    return np.max((y * x)[(y * x) % 3 == 0]) if np.any((y*x)[(y * x) % 3 == 0]) else -1


def convert_image(image: np.ndarray, weights: np.ndarray) -> np.ndarray:
    """
    Сложить каналы изображения с указанными весами.
    """
    return np.dot(image, weights)


def rle_scalar(x: np.ndarray, y: np.ndarray) -> int:
    """
    Найти скалярное произведение между векторами x и y, заданными в формате RLE.
    В случае несовпадения длин векторов вернуть -1.
    """
    a = np.repeat(x[:, 0], x[:, 1])
    b = np.repeat(y[:, 0], y[:, 1])
    return np.dot(a, b) if np.shape(a) == np.shape(b) else -1


def cosine_distance(X: np.ndarray, Y: np.ndarray) -> np.ndarray:
    """
    Вычислить матрицу косинусных расстояний между объектами X и Y.
    В случае равенства хотя бы одно из двух векторов 0, косинусное расстояние считать равным 1.
    """

    a = np.linalg.norm(X, axis=1)
    b = np.linalg.norm(Y, axis=1)
    norm = np.outer(a, b)
    norm1 = np.copy(norm)
    norm1[norm == 0] = 1
    result = X.dot(Y.T)/norm1
    result[norm == 0] = 1
    return result

