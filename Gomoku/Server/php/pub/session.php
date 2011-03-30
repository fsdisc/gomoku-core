<?php
require 'config.php';

$link = mysql_connect('localhost', $dbcfg['Username'], $dbcfg['Password']);
mysql_select_db($dbcfg['Database'], $link);

function table($name) {
  global $dbcfg;
  return $dbcfg['Prefix'].$name;
}

function stable($name) {
  global $dbcfg;
  return $dbcfg['SharedPrefix'].$name;
}

function quote($src) {
  return "'".mysql_real_escape_string($src)."'";
}

function execute($sql) {
  global $link;
  mysql_query($sql, $link);
}

function query($sql) {
  global $link;
  $target = array();
  $result = mysql_query($sql, $link);
  while ($row = mysql_fetch_assoc($result)) {
    $target[] = $row;
  }
  return $target;
}

function bit($src) {
  return $src ? 1 : 0;
}

function isPost() {
  $method = strtolower($_SERVER['REQUEST_METHOD']);
  return $method == 'post';
}

function post($name) {
  if (!isset($_POST[$name])) return '';
  return $_POST[$name];
}

function get($name) {
  if (!isset($_GET[$name])) return '';
  return $_GET[$name];
}

?>