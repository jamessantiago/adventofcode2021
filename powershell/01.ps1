#setup
$a=cat 01.txt

#day 1
$l=9;($a|?{$l-lt$_;$l=$_}).count

#day 2
(3..($a.Length-1)|?{$a[$_-3]-lt$a[$_]}).count