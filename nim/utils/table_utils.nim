import tables

func mergeInto*[T](into: CountTable[T], tables: seq[CountTable[T]]): CountTable[T] =
    result = initCountTable[T]()
    result.merge(into)
    for t in tables:
        result.merge(t)