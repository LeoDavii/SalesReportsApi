# SalesReports API

A Web API designed to provide comprehensive sales reports by parsing and analyzing sales record files.

---

## Overview

**SalesReports** is an API built to parse sales record files and generate detailed analytics including median unit costs, regional analysis, date ranges, and revenue calculations. This project was developed as part of a coding assessment to demonstrate architectural design, implementation skills, and development best practices.

---

## Features

- **File Processing:** Parse CSV sales record files into structured objects  
- **Statistical Analysis:** Calculate median unit costs from sales data  
- **Regional Analytics:** Identify the most common sales regions  
- **Date Range Analysis:** Determine first and last order dates with duration calculations  
- **Revenue Calculations:** Compute total revenue from sales records  
- **Containerized Deployment:** Docker support for easy local development and deployment  

---

## Quick Start

The template file is in the project root directory "SalesRecords.csv". To run the application locally, simply execute the following command in the project root directory. The API will be available at port 8080:

```bash
docker-compose up -d
