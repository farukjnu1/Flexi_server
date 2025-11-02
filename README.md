📱 GSM Modem Mobile Recharge Desktop App

A C# Desktop Application that allows users to refill mobile phone account balances directly through a GSM SIM modem.
This tool demonstrates how to interact with GSM modems for sending USSD commands or SMS requests to perform mobile recharges without using external APIs.

--------------------------------

🏗️ Overview

The GSM Modem Recharge App connects to a GSM modem (via COM port/USB) and sends recharge instructions to the mobile network.
It allows users to:

Top up a mobile account using the SIM in the GSM modem

Enter mobile number and recharge amount

Monitor transaction status via modem responses

This project is useful for offline recharge systems, telecom service providers, or personal recharge automation.

--------------------------------

🚀 Features
🔹 GSM Modem Integration

Connect via COM port to GSM SIM modem

Send USSD codes or SMS commands for recharge

Receive and parse modem response messages

💳 Mobile Recharge

Enter mobile number and amount

Select operator/network

Initiate recharge directly through the GSM SIM

🖥️ User Interface

Built with Windows Forms or WPF

Simple input form with operator selection, number, and amount

Display real-time status of recharge operations

📊 Transaction Logging

Log all recharge attempts and responses

Track success and failure for auditing

🔒 Security

Input validation (mobile number, amount limits)

Optional PIN or authentication for secure operation

---------------------------

🧱 Technologies Used
| Category               | Technology                               |
| ---------------------- | ---------------------------------------- |
| **Language**           | C#                                       |
| **Framework**          | .NET 6 / .NET 8                          |
| **UI Framework**       | Windows Forms / WPF                      |
| **Hardware Interface** | SerialPort (COM) for GSM modem           |
| **Recharge Mechanism** | USSD / SMS commands                      |
| **Optional Storage**   | SQLite / SQL Server for transaction logs |

-------------------

🧠 How It Works

The app connects to the GSM modem via SerialPort.

User enters recharge details (mobile number, amount, operator).

App formats and sends USSD command or SMS recharge code through the modem.

Modem sends the request to the network and receives a response.

App parses the response and displays success or failure.

All transactions are optionally logged for reference.

---------------------------

🔮 Future Enhancements

Add support for multiple SIM modems simultaneously

Auto-detect network/operator codes

Provide transaction history UI with search and filtering

Integrate with database or cloud backend for audit

Add scheduled recharge or batch processing
