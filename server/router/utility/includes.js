module.exports = function(list, item) {
    for (i = 0; i < list.length; i++) {
        if (list[i] == item)
            return true;
    }

    return false;
}