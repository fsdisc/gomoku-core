<?php
session_start();
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
    $data = query(sprintf('select * from %s where username = %s and password = %s', stable('users'), quote($username), quote(md5($password))));
    if (count($data) == 0) {
      $message = 'Username and password do not match!';
      $frame = 'error';
    } else {
      $userid = $data[0]['id'];

      $session = '';
      if (isset($_SESSION['session'])) {
        $session = $_SESSION['session'];
      }
      $online = false;
      $data = query(sprintf('select * from %s where id = %s', stable('session'), quote($session)));
      if (count($data) > 0) {
        $online = true;
      }
      if ($online) {
        execute(sprintf('delete from %s where id = %s', stable('session'), quote($session)));
      }

      $id = uniqid();
      execute(sprintf("insert into %s values (%s, %s, sysdate())", stable('session'), quote($id), quote($userid)));
      $_SESSION['session'] = $id;
      header('Location: index.php');
    }
  }
}


?>
<html>
<head>
  <title>Gomoku - Login</title>
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
      <td colspan='2' align='center'><input type='submit' value='Login'>&nbsp;&nbsp;<input type='button' value='Cancel' onclick='location.replace("index.php");'></td>
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
      <td align='center'><input type='button' value='Return' onclick='location.replace("login.php");'></td>
    </tr>
  </table>
</div>

<?php
}
?>

</body>
</html>