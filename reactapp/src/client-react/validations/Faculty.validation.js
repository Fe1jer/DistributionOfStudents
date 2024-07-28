import * as yup from 'yup';

export const FacultyValidationSchema = yup.object().shape({
    id: yup.string().uuid(),
    fullName: yup.string().required('Введите название'),
    shortName: yup.string().required('Введите название'),
    img: yup.string().default("\\img\\Faculties\\Default.jpg"),
    fileImg: yup.mixed().nullable(true),
});