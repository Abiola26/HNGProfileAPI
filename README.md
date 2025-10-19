# HNGProfileAPI

Welcome to my Stage 0 task submission for the **HNG Backend Wizards** cohort.  
This simple API returns my profile details and a dynamic cat fact fetched from an external API.

---

## 🚀 Endpoint

**GET** `/me`

### ✅ Example Response
```json
{
  "status": "success",
  "user": {
    "email": "youremail@example.com",
    "name": "Your Full Name",
    "stack": "ASP.NET Core"
  },
  "timestamp": "2025-10-16T12:34:56.789Z",
  "fact": "Cats have five toes on their front paws, but only four toes on their back paws."
}
