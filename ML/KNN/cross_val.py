import numpy as np
from collections import defaultdict


def kfold_split(num_objects, num_folds):
    """Split [0, 1, ..., num_objects - 1] into equal num_folds folds (last fold can be longer) and returns num_folds train-val
       pairs of indexes.

    Parameters:
    num_objects (int): number of objects in train set
    num_folds (int): number of folds for cross-validation split

    Returns:
    list((tuple(np.array, np.array))): list of length num_folds, where i-th element of list contains tuple of 2 numpy arrays,
                                       the 1st numpy array contains all indexes without i-th fold while the 2nd one contains
                                       i-th fold
    """
    result = []
    div = num_objects // num_folds
    for i in range(num_folds - 1):
        result.append(
            (
                np.array([j for j in range(num_objects) if (j < div * i) or (j >= div * (i + 1))]),
                np.array([j for j in range(num_objects) if (j >= div * i) and (j < div * (i + 1))])
            )
        )
    result.append(
        (
            np.array([j for j in range(num_objects) if (j < div * (num_folds - 1))]),
            np.array([j for j in range(num_objects) if (j >= div * (num_folds - 1))])
        )
    )
    return result


def knn_cv_score(X, y, parameters, score_function, folds, knn_class):
    """Takes train data, counts cross-validation score over grid of parameters (all possible parameters combinations)

    Parameters:
    X (2d np.array): train set
    y (1d np.array): train labels
    parameters (dict): dict with keys from {n_neighbors, metrics, weights, normalizers}, values of type list,
                       parameters['normalizers'] contains tuples (normalizer, normalizer_name), see parameters
                       example in your jupyter notebook
    score_function (callable): function with input (y_true, y_predict) which outputs score metric
    folds (list): output of kfold_split
    knn_class (obj): class of knn model to fit

    Returns:
    dict: key - tuple of (normalizer_name, n_neighbors, metric, weight), value - mean score over all folds
    """
    result = {}
    for n in parameters["n_neighbors"]:
        for m in parameters["metrics"]:
            for w in parameters["weights"]:
                for norm in parameters["normalizers"]:
                    score = 0
                    for i in folds:
                        X_train = X[i[0]]
                        X_valid = X[i[1]]
                        y_train = y[i[0]]
                        y_valid = y[i[1]]
                        model = knn_class(n_neighbors=n, metric=m, weights=w)
                        if norm[0]:
                            norm[0].fit(X_train)
                            X_train = norm[0].transform(X_train)
                            X_valid = norm[0].transform(X_valid)
                        model.fit(X_train, y_train)
                        y_predict = model.predict(X_valid)
                        score += score_function(y_valid, y_predict)
                    result[(norm[1], n, m, w)] = score / len(folds)
    return result
