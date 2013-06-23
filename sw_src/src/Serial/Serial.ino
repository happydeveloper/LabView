/*
  relay 
 */
 
// Pin 13 has an LED connected on most Arduino boards
// give it a name:
int feed = 13;

int incomingByte = 0;   // for incoming serial data

// the setup routine runs once when you press reset:
void setup() {
Serial.begin(9600);  
  // initialize the digital pin as an output.
  pinMode(feed, OUTPUT);     
}

// the loop routine runs over and over again forever:
void loop() {
  while(Serial.available() > 0) {
  incomingByte = Serial.read();
  Serial.println(incomingByte, DEC);
  Serial.println(incomingByte);
  if(incomingByte == 49){
       // say what you got:
       Serial.print("I start feed ");       
       digitalWrite(feed,HIGH);
       delay(1000);
       Serial.println(incomingByte, DEC);  
  } else {
    Serial.println("I stop feed: ");
    digitalWrite(feed, LOW);    // turn the LED off by making the voltage LOW
    delay(1000);   
  }
  }
}
