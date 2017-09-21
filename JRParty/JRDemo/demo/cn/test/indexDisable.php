<?php
echo 'hello wechat!';
header("location:../../JRGD/Wechat/monitor");


?>
<html>
<head>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8">
	<meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=0, minimum-scale=1.0, maximum-scale=1.0">
	<meta name="apple-mobile-web-app-capable" content="yes">
	<meta name="apple-mobile-web-app-status-bar-style" content="black">
</head>
<body>
<?php
for($i=0;$i<20;$i++){
	echo "<div style='width:100%;height:50px;margin-top:5px;margin-bottom:5px;border:1px solid blue;'>$i</div>";
}

?>
</body>
</html>
