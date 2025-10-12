import Card from 'react-bootstrap/Card';
import Col from 'react-bootstrap/Col';
import DeleteButton from '../adminButtons/DeleteButton';
import EditButton from '../adminButtons/EditButton';

import { Link } from 'react-router-dom'

export default function FacultyCard({ faculty, onClickDelete, onClickEdit }) {
    return (
        <Col xxl="3" sm="6" md="4" className="pt-2 pb-2">
            <Card className="shadow-sm">
                <div className="scale card-img__faculty">
                    <Link to={"/Faculties/" + faculty.shortName}>
                        <Card.Img className="scale" loading="lazy" src={faculty.imgThumb} alt={faculty.shortName} />
                    </Link>
                </div>
                <Card.Body>
                    <div className="box">
                        <div className="h-auto">
                            <Card.Title as="h4">{faculty.shortName}</Card.Title>
                            <Card.Text className="text-success">
                                {faculty.fullName}
                            </Card.Text >
                        </div>
                    </div>
                    <div className="d-flex justify-content-between align-items-center">
                        <Link type="button" className="btn btn-outline-success py-1" to={"/Faculties/" + faculty.shortName}>
                            Перейти
                        </Link>
                        <EditButton onClick={() => onClickEdit(faculty.shortName)} />
                        <DeleteButton onClick={() => onClickDelete(faculty.shortName, faculty.fullName)} />
                    </div>
                </Card.Body>
            </Card>
        </Col >
    );
}