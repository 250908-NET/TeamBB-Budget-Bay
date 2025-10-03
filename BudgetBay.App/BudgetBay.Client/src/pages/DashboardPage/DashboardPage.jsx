import './DashboardPage.module.css';

const DashboardPage = () => {
    return (
        <div className="dashboard-container">
            <h1>Protected Dashboard</h1>
            <p>You can only see this page if you are logged in.</p>
        </div>
    );
}

export default DashboardPage;