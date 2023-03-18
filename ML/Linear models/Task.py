import numpy as np


class Preprocessor:

    def __init__(self):
        pass

    def fit(self, X, Y=None):
        pass

    def transform(self, X):
        pass

    def fit_transform(self, X, Y=None):
        pass


class MyOneHotEncoder(Preprocessor):

    def __init__(self, dtype=np.float64):
        super(Preprocessor).__init__()
        self.dtype = dtype
        self.unique = []

    def fit(self, X, Y=None):
        """
        param X: training objects, pandas-dataframe, shape [n_objects, n_features]
        param Y: unused
        """
        mas = X.to_numpy()
        for i in mas.T:
            self.unique.append((np.unique(i)))

    def transform(self, X):
        """
        param X: objects to transform, pandas-dataframe, shape [n_objects, n_features]
        returns: transformed objects, numpy-array, shape [n_objects, |f1| + |f2| + ...]
        """
        mas = X.to_numpy()
        trans = None
        for i, vect in enumerate(mas.T):
            temp = np.zeros((mas.shape[0], len(self.unique[i])))
            for j, scal in enumerate(vect):
                temp[j][np.nonzero(self.unique[i] == scal)] = 1
            trans = np.hstack((trans, temp)) if trans is not None else temp
        return trans

    def fit_transform(self, X, Y=None):
        self.fit(X)
        return self.transform(X)

    def get_params(self, deep=True):
        return {"dtype": self.dtype}


class SimpleCounterEncoder:

    def __init__(self, dtype=np.float64):
        self.dtype = dtype
        self.unique = []
        self.X = []
        self.Y = []

    def fit(self, X, Y):
        """
        param X: training objects, pandas-dataframe, shape [n_objects, n_features]
        param Y: target for training objects, pandas-series, shape [n_objects,]
        """
        unique = []
        mas = X.to_numpy()
        for i in mas.T:
            unique.append(np.unique(i))
        self.X = X
        Y = Y.to_numpy()
        mas_dict = []
        for i in range(len(unique)):
            dict = {}
            for j in range(len(unique[i])):
                s = np.sum(Y[mas.T[i] == unique[i][j]])
                count = np.count_nonzero(mas.T[i] == unique[i][j])
                dict[unique[i][j]] = np.array([s / count, count / mas.shape[0], 0])
            mas_dict.append(dict)
        self.mas_dict = mas_dict

    def transform(self, X, a=1e-5, b=1e-5):
        """
        param X: objects to transform, pandas-dataframe, shape [n_objects, n_features]
        param a: constant for counters, float
        param b: constant for counters, float
        returns: transformed objects, numpy-array, shape [n_objects, 3]
        """
        result = None
        mas = X.to_numpy()
        for i, vect in enumerate(mas.T):
            temp = np.zeros((X.shape[0], 3))
            for j, scal in enumerate(vect):
                temp[j] = self.mas_dict[i][scal]
            temp[:, 2] = (temp[:, 0] + a) / (temp[:, 1] + b)
            result = np.hstack((result, temp)) if result is not None else temp
        return result

    def fit_transform(self, X, Y, a=1e-5, b=1e-5):
        self.fit(X, Y)
        return self.transform(X, a, b)

    def get_params(self, deep=True):
        return {"dtype": self.dtype}


def group_k_fold(size, n_splits=3, seed=1):
    idx = np.arange(size)
    np.random.seed(seed)
    idx = np.random.permutation(idx)
    n_ = size // n_splits
    for i in range(n_splits - 1):
        yield idx[i * n_: (i + 1) * n_], np.hstack((idx[:i * n_], idx[(i + 1) * n_:]))
    yield idx[(n_splits - 1) * n_:], idx[:(n_splits - 1) * n_]


class FoldCounters:

    def __init__(self, n_folds=3, dtype=np.float64):
        self.dtype = dtype
        self.n_folds = n_folds

    def fit(self, X, Y, seed=1):
        """
        param X: training objects, pandas-dataframe, shape [n_objects, n_features]
        param Y: target for training objects, pandas-series, shape [n_objects,]
        param seed: random seed, int
        """
        for_res = []
        self.split = group_k_fold(X.shape[0], self.n_folds, seed)
        for test, train in self.split:
            SimpleEncoder = SimpleCounterEncoder()
            SimpleEncoder.fit(X.iloc[train], Y.iloc[train])
            for_res.append((test, SimpleEncoder))

        self.for_res = for_res

    def transform(self, X, a=1e-5, b=1e-5):
        """
        param X: objects to transform, pandas-dataframe, shape [n_objects, n_features]
        param a: constant for counters, float
        param b: constant for counters, float
        returns: transformed objects, numpy-array, shape [n_objects, 3]
        """
        result = None
        for (index, fited) in self.for_res:
            temp = np.concatenate((np.array(index).reshape((-1, 1)), fited.transform(X.iloc[index], a, b)), axis=1)
            result = np.concatenate((result, temp), axis=0) if result is not None else temp

        return result[result[:, 0].argsort(), 1:]

    def fit_transform(self, X, Y, a=1e-5, b=1e-5):
        self.fit(X, Y)
        return self.transform(X, a, b)


def weights(x, y):
    """
    param x: training set of one feature, numpy-array, shape [n_objects,]
    param y: target for training objects, numpy-array, shape [n_objects,]
    returns: optimal weights, numpy-array, shape [|x unique values|,]
    """
    unique, count = np.unique(x, return_counts=True)
    w = np.array([0.0]*count)
    for i, elem in enumerate(unique):
        w[i] = sum(y[x == elem] / count[i])
    return w
