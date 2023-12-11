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

proc flatten*[T](seqs: seq[seq[T]]): seq[T] =
  result = @[]
  for subseq in seqs:
    for item in subseq:
      result.add(item)

proc window*[T](s: seq[T], size: int): seq[seq[T]] =
  result = @[]
  for i in 0 ..< len(s) - size + 1:
    result.add(s[i ..< i + size])

proc pairs*[T](s: seq[T]): seq[(T, T)] =
  result = @[]
  for i in 0 ..< len(s):
    for j in i + 1 ..< len(s):
      result.add((s[i], s[j]))