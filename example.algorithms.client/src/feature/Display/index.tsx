import { AppBar, Container, Paper, Tab, Tabs, Typography } from '@material-ui/core'
import React, { useState } from 'react'
import QSort from '../QSort'


enum Selections {
    QSort = "qsort",
    EditDistance = "editDistance"
}



export interface DisplayProps { }
const Display: React.FunctionComponent<DisplayProps> = () => {

    const [selection, setSelection] = useState<Selections>(Selections.QSort)

    const handleSelect = (event: React.ChangeEvent<{}>, value:any) => {
        setSelection(value as Selections)
    }

    return (
        <>
            <Container fixed >
                <Paper style={{ padding: "15px" }}>
                    <AppBar position="static">
                        <Tabs value={selection} onChange={handleSelect} aria-label="simple tabs example">
                            <Tab label="QSort" value={Selections.QSort} />
                            <Tab label="Edit Distance" value={Selections.EditDistance} />
                        </Tabs>
                    </AppBar>
                    {selection == Selections.QSort && <QSort />}
                    {selection == Selections.EditDistance && <div />}
                </Paper>
            </Container>
        </>
    )
}
export default Display