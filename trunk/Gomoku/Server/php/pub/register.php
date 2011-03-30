<?php
require('session.php');

$message = '';
$frame = 'input';

if (isPost()) {
  $username = trim(post('username'));
  $password = trim(post('password'));
  if (strlen($username) == 0) {
    $frame = 'error';
    $message = 'Username is required!';
  } else if (strlen($password) == 0) {
    $frame = 'error';
    $message = 'Password is required!';
  } else {
    $data = query(sprintf('select * from %s where username = %s', stable('users'), quote($username)));
    if (count($data) > 0) {
      $message = 'Username already exists!';
      $frame = 'error';
    } else {
      execute(sprintf("insert into %s values (%s, %s, %s)", stable('users'), quote(uniqid()), quote($username), quote(md5($password))));
      header('Location: index.php');
    }
  }
}


?>
<html>
<head>
  <title>Gomoku - Register</title>
  <style>
div, td { font-family: sans-serif; font-size: 10pt; }
.frame { border: solid 1px silver; border-left: solid 5px red; width: 220px; height: 100px; margin: 50px auto 0 auto; padding: 10px; }
  </style>
</head>
<body>

<?php
if ($frame == 'input') {
?>

<form method='post'>
<div class='frame'>
  <table>
    <tr>
      <td>Username</td>
      <td><input type='text' name='username'></td>
    </tr>
    <tr>
      <td>Password</td>
      <td><input type='password' name='password'></td>
    </tr>
    <tr>
      <td colspan='2'>&nbsp;</td>
    </tr>
    <tr>
      <td colspan='2' align='center'><input type='submit' value='Register'>&nbsp;&nbsp;<input type='button' value='Cancel' onclick='location.replace("index.php");'></td>
    </tr>
  </table>
</div>  
</form>

<?php
}
if ($frame == 'error') {
?>

<div class='frame'>
  <table>
    <tr><td>&nbsp;</td></tr>
    <tr>
      <td><?php print($message); ?></td>
    </tr>
    <tr><td>&nbsp;</td></tr>
    <tr>
      <td align='center'><input type='button' value='Return' onclick='location.replace("register.php");'></td>
    </tr>
  </table>
</div>

<?php
}
?>

</body>
</html>