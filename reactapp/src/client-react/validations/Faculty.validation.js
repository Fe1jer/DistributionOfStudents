import * as yup from 'yup';

export const FacultyValidationSchema = yup.object().shape({
    id: yup.string().matches(/^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$/i),
    fullName: yup.string().required('Введите название'),
    shortName: yup.string().required('Введите название'),
    img: yup.string().default("\\img\\Faculties\\Default.jpg"),
    fileImg: yup.mixed().nullable(true),
});