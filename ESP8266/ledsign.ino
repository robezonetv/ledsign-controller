#include <stdio.h>
#include <ESP8266WiFi.h>
#include <WiFiClient.h>
#include <ESP8266WebServer.h>
#include <ESP8266mDNS.h>
#include <IRremoteESP8266.h>
#include <IRsend.h>
#include <EasyButton.h>
#include "LittleFS.h"

// FLASH Button for reset configuration
#define BUTTON_PIN 0
EasyButton button(BUTTON_PIN);

// Definice LED na WiFiBoardu
#define LEDka LED_BUILTIN

// IR Dioda pro posilani signalu
const uint16_t kIrLed = 4;
IRsend irsend(kIrLed);

// Defaultni konfigurace WiFi pro AP
const char* ap_ssid = "LEDSign";
const char* ap_passwd = "robezonetv";

// Definovane IR kody ovladace (bez moznosti nacteni)
// source: https://github.com/crankyoldgit/IRremoteESP8266
unsigned long keys[24] = {
0xF700FF,
0xF7807F,
0xF740BF,
0xF7C03F,
0xF720DF,
0xF7A05F,
0xF7609F,
0xF7E01F,
0xF710EF,
0xF7906F,
0xF750AF,
0xF7D02F,
0xF730CF,
0xF7B04F,
0xF7708F,
0xF7F00F,
0xF708F7,
0xF78877,
0xF748B7,
0xF7C837,
0xF728D7,
0xF7A857,
0xF76897,
0xF7E817
};

// Webovy server
// source: https://navody.dratek.cz/navody-k-produktum/esp-wifi-webserver.html
ESP8266WebServer server(80);

// =============== FUNKCE ================

// Zpracovani stisknuti tlacitka
// source: https://github.com/evert-arias/EasyButton/blob/main/examples/PressedForDuration/PressedForDuration.ino
void onPressed() {
  for (int i=0; i<5; i++) {
    digitalWrite(LEDka, LOW);
    delay(500);
    digitalWrite(LEDka, HIGH);
    delay(500);
  }
  Serial.println("Removing WiFi configuration and reboot ...");
  LittleFS.remove("last_millis.txt");
  LittleFS.remove("wifi.config");
  Serial.println("Rebooting ESP in 1 second...");
  delay(1000);
  ESP.restart();
}

// Uvodni stranka pro nastaveni WiFi
void indexAPPage() {
  String cas = String(millis() / 1000);
  String zprava = "<!DOCTYPE html><html lang=\"en\"><head><meta charset=\"UTF-8\"><meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\"><meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\"><title>LEDSign by robezonetv</title></head><body>";
  zprava += "<center><h1>LEDSign control</h1><h2><a href=\"https://twitch.tv/robezonetv\">made with &#10084; twitch.tv/robezonetv</a></h2><h3>uptime ";
  zprava += cas;
  zprava += " s</h3>";
  zprava += "<form method=\"get\" action=\"/store\">SSID: <input type=\"text\" name=\"ssid\" id=\"ssid\" /><br>Password: <input type=\"password\" name=\"password\" id=\"password\" /><br><input type=\"submit\" value=\"Save WiFi configuration\"></form></center></body></html>";
  server.send(200, "text/html", zprava);
}

// Informativni stranka ohledne ulozeni WiFi
void storedAPPage() {
  String cas = String(millis() / 1000);
  String zprava = "<!DOCTYPE html><html lang=\"en\"><head><meta charset=\"UTF-8\"><meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\"><meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\"><title>LEDSign by robezonetv</title></head><body>";
  zprava += "<center><h1>LEDSign control</h1><h2><a href=\"https://twitch.tv/robezonetv\">made with &#10084; twitch.tv/robezonetv</a></h2><h3>uptime ";
  zprava += cas;
  zprava += " s</h3>";
  zprava += "<h2>WiFi stored!</h2><h3>Automatic restart in 5 seconds...</h3></center></body></html>";
  server.send(200, "text/html", zprava);
}

// Zakladni stranka pro ovladani LED svetel
void indexHTML() {
  String cas = String(millis() / 1000);
  String zprava = "<!DOCTYPE html><html lang=\"en\"><head><meta charset=\"UTF-8\"><meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\"><meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\"><title>LEDSign by robezonetv</title><style>/* moiasx helped with CSS in first version of controller ;) */a{text-decoration: none;color: #000;}.wrapper {display: flex;justify-content: center;align-items: center;}.controller {display: flex;flex-wrap: wrap;width: 400px;gap: 15px;border: 5px solid;border-radius: 20px;box-sizing: border-box;padding: 30px 20px;font-size: 1.4rem;font-weight: bold;}.controller >a {flex: 1 1 20%;display: block;border-radius: 50%;width: 100%;aspect-ratio: 1/1;text-decoration: none;text-align: center;position: relative;}a >div {position: absolute;top: 50%;left: 50%;transform: translate(-50%, -50%);}.item1{border: #000 1px solid;}.item2{border: #000 1px solid;}.item3{color: #fff;background-color: #000;}.item4 {background-color: #ff0000;color: #fff;}.item5{background-color: #ff0000;}.item6{background-color: #00ff00;}.item7{background-color: #0000ff;}.item8{border: #000 1px solid;}.item9{background-color: #c71628;}.item10{background-color: #019454;}.item11{background-color:#0b66a9;}.item12 {background-color: #8d8d8d;font-size: 1.3rem;}.item13{background-color: #cc3832;}.item14{background-color: #0cacb0;}.item15{background-color:#381d4c;}.item16 {background-color: #8d8d8d;font-size: 1.1rem;}.item17{background-color: #dc7048;}.item18{background-color: #1c8495;}.item19{background-color: #592a65;}.item20 {background-color: #8d8d8d;}.item21{background-color: #f5df4d;}.item22{background-color: #0a5669;}.item23{background-color: #a02773;}.item24 {background-color: #8d8d8d;font-size: 1rem;}@media screen and (max-width: 500px) {.wrapper {height: fit-content;}.controller {width: 100vh;height: max-content;}}</style></head><body>";
  zprava += "<center><h1>LEDSign control</h1><h2><a href=\"https://twitch.tv/robezonetv\">made with &#10084; twitch.tv/robezonetv</a></h2><h3>uptime ";
  zprava += cas;
  zprava += " s<br>IP Adresa pro LEDSign pro Twitch: " + WiFi.localIP().toString() + "</h3></center>";
  zprava += "<div class=\"wrapper\"><div class=\"controller\"><a href=\"/led?key=0\" class=\"item1\"><div>JAS+</div></a><a href=\"/led?key=1\" class=\"item2\"><div>JAS-</div></a><a href=\"/led?key=2\" class=\"item3\"><div>OFF</div></a><a href=\"/led?key=3\" class=\"item4\"><div>ON</div></a><a href=\"/led?key=4\" class=\"item5\"><div></div></a><a href=\"/led?key=5\" class=\"item6\"><div></div></a><a href=\"/led?key=6\" class=\"item7\"><div></div></a><a href=\"/led?key=7\" class=\"item8\"><div></div></a><a href=\"/led?key=8\" class=\"item9\"><div></div></a><a href=\"/led?key=9\" class=\"item10\"><div></div></a><a href=\"/led?key=10\" class=\"item11\"><div></div></a><a href=\"/led?key=11\" class=\"item12\"><div>FLASH</div></a><a href=\"/led?key=12\" class=\"item13\"><div></div></a><a href=\"/led?key=13\" class=\"item14\"><div></div></a><a href=\"/led?key=14\" class=\"item15\"><div></div></a><a href=\"/led?key=15\" class=\"item16\"><div>STROBE</div></a><a href=\"/led?key=16\" class=\"item17\"><div></div></a><a href=\"/led?key=17\" class=\"item18\"><div></div></a><a href=\"/led?key=18\" class=\"item19\"><div></div></a><a href=\"/led?key=19\" class=\"item20\"><div>FADE</div></a><a href=\"/led?key=20\" class=\"item21\"><div></div></a><a href=\"/led?key=21\" class=\"item22\"><div></div></a><a href=\"/led?key=22\" class=\"item23\"><div></div></a><a href=\"/led?key=23\" class=\"item24\"><div>SMOOTH</div></a></div></div><center><i>controller by zaky_py</i></center></body></html>";
  server.send(200, "text/html", zprava);
}

// Chybova stranka, ktera udela redirect na hlavni
void error404() {
  server.sendHeader("Location", String("/"), true);
  server.send ( 302, "text/plain", "");
}

// Nastavit jako AP pro pripojeni mobilem
void startWifiAsAP(){
  Serial.print("Setting AP (Access Point)...");
  WiFi.softAP(ap_ssid, ap_passwd);
  IPAddress IP = WiFi.softAPIP();
  Serial.print("AP IP address: ");
  Serial.println(IP);

  if (MDNS.begin("ledsign")) {
    Serial.println("MDNS responder je zapnuty (ledsign.local) ... ");
  }

  server.on("/", indexAPPage);

  server.on("/store", []() {
    String wifi_ssid = server.arg("ssid");
    String wifi_pass = server.arg("password");
    write_to_file("wifi.config", wifi_ssid + "\n" + wifi_pass);
    delay(500);
    server.sendHeader("Location", String("/stored"), true);
    server.send(302, "text/plain", "");
  });

  server.on("/stored", []() {
    // Prevent reset ESP when stored is cached on mobile
    // from previous run.
    // Return to main page if configuration not exists
    String wifi_config = load_from_file("wifi.config");
    if (wifi_config == "") {
      server.sendHeader("Location", String("/"), true);
      server.send(302, "text/plain", "");
    } 
    else {
      storedAPPage();
      Serial.println("WiFi konfigurace ulozena ...");
      Serial.println("Reboot in 5 seconds ...");
      for (int i=0; i<5; i++) {
        digitalWrite(LEDka, LOW);
        delay(500);
        digitalWrite(LEDka, HIGH);
        delay(500);
      }
      ESP.restart();
    }
  });

  server.onNotFound(error404);

  server.begin();
}

// Pripojit se na WiFi, kterou jsme nakonfigurovali.
// Nefunguje? Proste resni ESP pres 5s tlacitko
void startWifiAsStation(String nazevWifi, String hesloWifi){
  WiFi.begin(nazevWifi, hesloWifi);
  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
  }
  
  Serial.println("");
  Serial.print("Pripojeno k WiFi siti ");
  Serial.println(nazevWifi);
  Serial.print("IP adresa: ");
  Serial.println(WiFi.localIP());

  if (MDNS.begin("ledsign")) {
    Serial.println("MDNS responder je zapnuty (ledsign.local) ... ");
  }

  server.on("/", indexHTML);

  server.on("/led", []() {
    String value = server.arg("key");
    int input = value.toInt();
    Serial.println("value from arg["+value+"]");
    Serial.println(keys[input], HEX);
    irsend.sendNEC(keys[input]);
    indexHTML();
  });

  server.onNotFound(error404);

  server.begin();
  Serial.println("HTTP server je zapnuty.");
}

// Reading and Writing configuration from file
// source: https://handyman.dulare.com/save-configuration-data-in-the-file-system-of-esp8266/
String load_from_file(String file_name) {
  String result = "";
  
  File this_file = LittleFS.open(file_name, "r");
  if (!this_file) { // failed to open the file, retrn empty result
    return result;
  }
  while (this_file.available()) {
      result += (char)this_file.read();
  }
  
  this_file.close();
  return result;
}

bool write_to_file(String file_name, String contents) {
  File this_file = LittleFS.open(file_name, "w");
  if (!this_file) { // failed to open the file, return false
    return false;
  }
  int bytesWritten = this_file.print(contents);
 
  if (bytesWritten == 0) { // write failed
      return false;
  }
   
  this_file.close();
  return true;
}

// Parsing String
// source: https://arduino.stackexchange.com/a/1237
String getValue(String data, char separator, int index)
{
    int found = 0;
    int strIndex[] = { 0, -1 };
    int maxIndex = data.length() - 1;

    for (int i = 0; i <= maxIndex && found <= index; i++) {
        if (data.charAt(i) == separator || i == maxIndex) {
            found++;
            strIndex[0] = strIndex[1] + 1;
            strIndex[1] = (i == maxIndex) ? i+1 : i;
        }
    }
    return found > index ? data.substring(strIndex[0], strIndex[1]) : "";
}

void setup() {
  // Seriova linka
  Serial.begin(115200, SERIAL_8N1);

  // WiFi LED status
  pinMode(LEDka, OUTPUT);
  digitalWrite(LEDka, HIGH);

  // Enable filesystem in Flash memory
  LittleFS.begin();

  // Load WiFi configuration
  String wifi_config = load_from_file("wifi.config");
  if (wifi_config == "") {
    Serial.println("Starting WiFi as AP ...");
    startWifiAsAP();
  }
  else {
    String myssid = getValue(wifi_config, '\n', 0);
    String mypass = getValue(wifi_config, '\n', 1);
    Serial.println("Starting WiFi as Station ...");
    startWifiAsStation(myssid, mypass);
  }

  // IR controller
  irsend.begin();

  // Handle restart after 5 seconds of button pressed
  button.begin();
  button.onPressedFor(5000, onPressed);
}

void loop() {
  server.handleClient();
  button.read();
  MDNS.update();
  delay(10);
}
