using System;

namespace _70_487_Examples.Models
{
    public class CachingModel
    {
        private string _value;
        private DateTime _setDate;
        private DateTime expireDate; 
        private TimeSpan _expiry = new TimeSpan(0, 0, 30);
        private bool isSliding; 

        public CachingModel(string value, bool sliding){
            Value = value;
            this.isSliding = sliding;            
        }

        public string Value { 
            get { 
                if(isSliding){ 
                    setExpiration(); 
                }
                return this._value; 
            }
            set { 
                this._value = value; 
                this._setDate = DateTime.Now;
                setExpiration();                
            }
        }

        public string ExpirationDate{
            get {                
                return this.expireDate.ToString();
            }
        }

        private void setExpiration(){
            if(this.isSliding){
                this._setDate = DateTime.Now;
            }
            this.expireDate = this._setDate.Add(this._expiry); 
        }
    }
}