#include <Wire.h> 
#include <LiquidCrystal_I2C.h>
#include <ESP8266WiFi.h>
#include <ESP8266HTTPClient.h>

// MCU pins mapping
#define D3 0 // GPIO0 
#define D4 2 // GPIO2

// LCD configuration
#define LCD_I2C_ADDRESS 0x27
#define LCD_COLUMNS_COUNT 16
#define LCD_ROWS_COUNT 2
#define LCD_SDA D4
#define LCD_SCL D3
LiquidCrystal_I2C lcd(LCD_I2C_ADDRESS, LCD_COLUMNS_COUNT, LCD_ROWS_COUNT);

// WIFI configuration
#define IS_WIFI_ENABLED true
#define WIFI_RECONNECTION_TIME_MS 1000
const char* ssid = "casper4";
const char* password = "myhata42";

//-----------------------------------------------------------------------------------

#define DISPLAY_UPDATES_FREQUENCY_MS 2000
#define DISPLAY_API_URL "http://52.59.20.223:6061/weather"
const int photopin = A0;
const int ledpin = D1;
const int max_brightness = 750;
const int min_brightness = 250;
int lastDisplayUpdateMs;

//-----------------------------------------------------------------------------------

void setupBacklightBrightness() {
  pinMode(ledpin, OUTPUT);
  pinMode(photopin, INPUT);
}

//-----------------------------------------------------------------------------------

void updateBacklightBrightness() {
  int brightness = analogRead(photopin);
  brightness = constrain(brightness, min_brightness, max_brightness);
  int ledPower = map(brightness, min_brightness, max_brightness, 50, 1024);
  analogWrite(ledpin, ledPower);
}

//-----------------------------------------------------------------------------------

void setupLiquidCrystalDisplay() {
  Wire.begin(LCD_SDA, LCD_SCL);
  lcd.init();
  lcd.backlight();
  lcd.home();  

  if (IS_WIFI_ENABLED) {
    WiFi.begin(ssid, password);
    while (WiFi.status() != WL_CONNECTED) {
      delay(WIFI_RECONNECTION_TIME_MS); 
    }
  }
}

String getDisplayString() {
  if (!IS_WIFI_ENABLED) {
    return "WIFI            DISABLED";
  }
  if (WiFi.status() != WL_CONNECTED) {
    return "WIFI            DISCONNECTED";
  }
  HTTPClient http;
  String responseString;
  http.begin(DISPLAY_API_URL);
  int httpCode = http.GET(); 
  if (httpCode > 0) {
    responseString = http.getString();
  } else {
    responseString = "API             FAILED";
  }
  http.end();
  return responseString;
}

int updateLiquidCrystalDisplay() {
  if (millis() - lastDisplayUpdateMs <= DISPLAY_UPDATES_FREQUENCY_MS) {
    return 0;
  }
  lastDisplayUpdateMs = millis();
  String displayString = getDisplayString();
  String lcdLines[2];
  lcdLines[0] = displayString.substring(0, LCD_COLUMNS_COUNT);
  lcdLines[1] = displayString.substring(LCD_COLUMNS_COUNT, LCD_COLUMNS_COUNT * 2);
  lcd.clear();
  lcd.print(lcdLines[0]);
  lcd.setCursor(0, 1);
  lcd.print(lcdLines[1]);
  return 1;
}

//-----------------------------------------------------------------------------------

void setup() {
  setupBacklightBrightness();
  setupLiquidCrystalDisplay();
}

//-----------------------------------------------------------------------------------

void loop() {
  updateLiquidCrystalDisplay();
  updateBacklightBrightness();
}
