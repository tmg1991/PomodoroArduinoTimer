
#include "SevenSegmentTM1637.h"

int pinled = 4;
const byte PIN_CLK = 2;
const byte PIN_DIO = 3;
SevenSegmentTM1637    display(PIN_CLK, PIN_DIO);

void setup() {
  pinMode(pinled, OUTPUT);
  Serial.begin(9600);
  display.begin();
  display.setBacklight(100);
};


void loop() {
    char inByte = Serial.read();
    if(inByte == 'S')
    {
      StopTimer();
    }

    if(inByte == 't')
    {
      String minuteString = Serial.readString();
      int minutes = minuteString.toInt();
      int seconds = minutes * 60;
      while(seconds >= 0)
      {
        char inByte = Serial.read();
        if(inByte == 'S')
        {
          break;
        }
        DisplaySeconds(seconds);
        seconds--;
        delay(1000);
      }
      BlinkLED();
      StopTimer();
    }
};

void DisplaySeconds(int seconds)
{
  display.setColonOn(true);
  int minutes = seconds / 60;
  int remainder = seconds - minutes * 60;
  String minuteString;
  if(minutes < 10)
  {
    minuteString = String(0) + String(minutes);
  }
  else
  {
    minuteString =  String(minutes);    
  }

  String secondsString;
  if(remainder < 10)
  {
    secondsString = String(0) + String(remainder);
  }
  else
  {
    secondsString =  String(remainder);    
  }
  String time = minuteString + secondsString;
  display.print(time);

}

void StopTimer()
{
  display.setColonOn(false);
  display.clear();
}

void BlinkLED()
{
  for(int i = 0; i<4; i++)
  {
    digitalWrite(pinled, HIGH);   // turn the LED on 
		delay(100);               // wait for a second
		digitalWrite(pinled, LOW);    // turn the LED off 
		delay(100);   
  }
}

