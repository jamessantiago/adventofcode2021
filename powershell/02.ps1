#setup
$a=cat 02.txt |% {$x = $_.split(' ');$xn = "" | Select D,A;$xn.D = $x[0]; $xn.A = [int]$x[1];$xn};$f="forward";$d="down";$u="up"

#day 1
$a|?{$_.D-eq$f}|%{$h+=$_.A};$a|?{$_.D-eq$d}|%{$v+=$_.A};$a|?{$_.D-eq$u}|%{$v-=$_.A};$h*$v

#day 2
$a|%{if($_.D-eq$f){$o+=$_.A;$e+=$_.A*$m}elseif($_.D-eq$d){$m+=$_.A}else{$m-=$_.A}};$o*$e