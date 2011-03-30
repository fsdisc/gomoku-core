<?php
require('../session.php');

$username = get('u');
$password = get('p');

$stop = false;

$data = query(sprintf('select * from %s where username = %s and password = %s', stable('users'), quote($username), quote(md5($password))));
if (count($data) == 0) {
  print('Error: Username and password does not match!');
  $stop = true;
}

if (!$stop) {
  $userid = $data[0]['id'];
  $id = uniqid();
  execute(sprintf('insert into %s values (%s, %s, sysdate())', stable('session'), quote($id), quote($userid)));
  print('Success: ' . $id);
}

?>