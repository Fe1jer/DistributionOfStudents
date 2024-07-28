import Button from 'react-bootstrap/Button';

import { useSelector } from 'react-redux';

export default function CreateButton({ className, size, onClick }) {
    const authUser = useSelector(x => x.auth.user);

    if (authUser && authUser.role === "commission") {
        return <Button variant="empty" size={size} className={className + " p-0 text-success"} onClick={onClick} >
            <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" fill="currentColor" className="bi bi-plus-circle-fill suc" viewBox="0 0 16 16">
                <path d="M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0zM8.5 4.5a.5.5 0 0 0-1 0v3h-3a.5.5 0 0 0 0 1h3v3a.5.5 0 0 0 1 0v-3h3a.5.5 0 0 0 0-1h-3v-3z" />
            </svg>
        </Button >
    }
}