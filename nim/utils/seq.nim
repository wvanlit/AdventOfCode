import sequtils

proc swap[T](a: var seq[T], i, j: int) =
    let temp = a[i]
    a[i] = a[j]
    a[j] = temp

proc permute[T](s: var seq[T], start: int): seq[seq[T]] =
    result = @[]

    if start == len(s):
        result = @[s]
    else:
        for i in start ..< len(s):
            s.swap(start, i)
            result.add(permute(s, start + 1))
            s.swap(start, i)
   

proc permutations*[T](s: var seq[T]): seq[seq[T]] =
    result = permute(s, 0)

 