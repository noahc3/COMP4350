import { Box, Flex } from '@chakra-ui/react';
import { observer } from 'mobx-react';
import { authStore } from '../../stores/AuthStore';
import { AuthGate } from '../AuthGate/AuthGate';
import { Router } from '../Router/Router';
import { Sidebar } from '../Sidebar/Sidebar';
import './App.css';

const App = observer(() => {
    const isAuthenticated = authStore.isAuthenticated;

    const content = isAuthenticated ? (
        <>
            <Flex>
                <Box flexBasis={"content"}>
                    <Sidebar/>
                </Box>
                <Box flexGrow={1}>
                    <Router/>
                </Box>
            </Flex>
        </>
    ) : (
        <Box>
            <Router/>
        </Box>
    )

    return (
        <div className="App">
            <AuthGate>
                {content}
            </AuthGate>
        </div>
    );
});

export default App;
