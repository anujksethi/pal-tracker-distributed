DROP DATABASE IF EXISTS tracker_allocations_dotnet_dev;
DROP DATABASE IF EXISTS tracker_backlog_dotnet_dev;
DROP DATABASE IF EXISTS tracker_registration_dotnet_dev;
DROP DATABASE IF EXISTS tracker_timesheets_dotnet_dev;
DROP DATABASE IF EXISTS tracker_allocations_dotnet_test;
DROP DATABASE IF EXISTS tracker_backlog_dotnet_test;
DROP DATABASE IF EXISTS tracker_registration_dotnet_test;
DROP DATABASE IF EXISTS tracker_timesheets_dotnet_test;

CREATE USER IF NOT EXISTS 'tracker_dotnet'@'localhost'
  IDENTIFIED BY 'password';

GRANT ALL PRIVILEGES ON *.* TO 'tracker_dotnet' @'localhost';

CREATE DATABASE tracker_allocations_dotnet_dev;
CREATE DATABASE tracker_backlog_dotnet_dev;
CREATE DATABASE tracker_registration_dotnet_dev;
CREATE DATABASE tracker_timesheets_dotnet_dev;
CREATE DATABASE tracker_allocations_dotnet_test;
CREATE DATABASE tracker_backlog_dotnet_test;
CREATE DATABASE tracker_registration_dotnet_test;
CREATE DATABASE tracker_timesheets_dotnet_test;