#include <iostream>
#include <thread>
#include <mutex>
class SensorData {
public:
    int temperature;
 
    SensorData(int temp) : temperature(temp) {
        std::cout << "SensorData created with temperature: " << temperature << std::endl;
    }
 
    void printTemperature() {
        std::cout << "Current temperature: " << temperature << std::endl;
    }
 
    ~SensorData() {
        std::cout << "SensorData object destroyed!" << std::endl;
    }
};
 
void processData(SensorData* data) {
    if (data!=nullptr) {
        data->printTemperature();
    } else {
        std::cout << "Invalid SensorData pointer." << std::endl;
    }
}
 
int main() {
    SensorData* sensor1 = new SensorData(25);
    processData(sensor1);
    delete sensor1;
    sensor1 = nullptr;
    SensorData* sensor2 = new SensorData(15);
    processData(sensor1);    
    delete sensor2;
    sensor2 = nullptr;
    return 0;
}
 
