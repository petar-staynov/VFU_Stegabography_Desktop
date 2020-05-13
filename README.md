# Image Steganography Application
WFP desktop image steganography program for Windows 64-bit based on .NET Core 3.1

## What does it do?
It allows users to hide a secret message in a bitmap (.bmp) image. The message can optionally be encrypted with a password for extra security.

## How does it work?
1. Loop through every pixel of the image
2. Get the value of every color channel (Red-Green-Blue) and clear its last bit (least significant bit) by filling them with 0 (zero)
3. Take a character from the user input message string and convert it to an 8-bit integer
4. Hide 1 bit of this inetegr into every color channel for the next 3 pixels - R1, G1, B1, R2, G2, B2, R3, G3
5. After processing the 8 bits of the character, move to the next one and repeat from step 3.
6. To mark the end of the text, write 8 consecutive zeroes (in the next 24 pixels). This will be used when reading the message.


## How do I sue it?
### Hiding data:
1. Download it from the [Releases page](https://github.com/petar-staynov/VFU_Stegabography_Desktop/releases) or compile it yourself with Visual Studio 2019
2. Click "Browse" and open a .bmp image
3. Write the text you want to embed, in the input field. 
4. Optionally you can encrypt it with a password by ticking the "Encrypt" box and writing a password in the "Encrypt password" field.
5. Click Save to save the new file.

### Reading hidden data:
1. Click "Browse" and open a .bmp image that was made using this program.
2. If the hidden text was encrypted, type your decryption password in the "Decrypt password" field, otherwise leave blank.
3. Click "Read Message" to show the data in the input box.
4. If you get a random string of letters either you didn't supply a password or the file doesn't have a hidden text in it.

## Images:
<p align="center">
  <img src="./images/ui.png?raw=true" width=auto title="Steganography app image">
</p>

## Used resources:
[Hamzeh soboh (Codeproject) - Steganography Simple Implementation](https://www.codeproject.com/Tips/635715/Steganography-Simple-Implementation-in-Csharp)

[Carlos Delgado (OurCodeWorld) - Getting Started With Steganography](https://ourcodeworld.com/articles/read/474/getting-started-with-steganography-hide-information-on-images-with-c-sharp#disqus_thread)

[Computerphile (YouTube) - Secrets Hidden in Images (Steganography)](https://www.youtube.com/watch?v=TWEXCYQKyDc)
