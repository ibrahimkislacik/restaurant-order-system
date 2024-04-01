export class Logger {

    static logLevel?: number;

    static getLogLevel(): number {
        if (!Logger.logLevel) {
            Logger.logLevel = 0;
            const logLevel = Number(localStorage.getItem('logLevel'));
            if (!isNaN(logLevel)) {
                Logger.logLevel = logLevel;
            }
        }
        return Logger.logLevel;
    }

    static info(label: string, message: any, ...optionalParams: any[]): void {
        if (label) {
            console.log(`- ${label}`);
        }
        console.info(message, ...optionalParams);
        console.log(' ');
    }

    static warn(label: string, message: any, ...optionalParams: any[]): void {
        if (Logger.getLogLevel() < 1) {
            return;
        }
        if (label) {
            console.log(`- ${label}`);
        }
        console.warn(message, ...optionalParams);
        console.log(' ');
    }

    static error(label: string, message: any, ...optionalParams: any[]): void {
        if (label) {
            console.log(`- ${label}`);
        }
        console.error(message, ...optionalParams);
        console.log(' ');
    }
}
